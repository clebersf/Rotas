# 4. Camada de Apresentação (WPF UI)

O projeto **`Vale.Rotas`** (desenvolvido em C# com **Windows Presentation Foundation - WPF**) é o painel de comando do operador (CCO). O propósito deste aplicativo é ler os dados do banco, exibir a planta em tempo real e permitir que o operador autorize ou cancele o roteamento do minério.

Como se trata de um sistema industrial crítico, o código precisa ser **à prova de falhas do usuário** (como clicar duas vezes em "Iniciar" ou abrir o aplicativo duas vezes sem querer). Abaixo explicamos as engrenagens por trás das telas.

---

## 4.1. O Guarda-Costas do Sistema (`App.xaml.cs`)

O arquivo `App.xaml.cs` é o primeiro código a rodar quando o operador dá um duplo-clique no ícone do sistema. Ele atua como um "guarda-costas" garantindo a estabilidade.

* **O Problema das Múltiplas Instâncias:** 
  Se o operador abrir o "Sistema Rotas" duas vezes, teremos dois programas tentando ler e escrever no banco ao mesmo tempo, causando conflitos (*locks*).
* **A Solução (O `Mutex`):**
  Logo no método `App_Startup`, o código usa um `Mutex` (uma "chave" global do Windows). 
  ```csharp
  _mutex = new Mutex(true, "Global\\" + AppGuid, out createdNew);
Se a variável createdNew for falsa, significa que o programa já está aberto. O guarda-costas então usa ferramentas nativas do Windows (user32.dll -> ForceForegroundWindow) para piscar a tela que já estava aberta e puxá-la para a frente, fechando a segunda cópia imediatamente.
Ocultar em vez de Fechar (App_Deactivated e WM_CLOSE):
Se o operador clicar no "X" vermelho sem querer, o sistema não desliga. Ele intercepta o evento do Windows e apenas minimiza ou oculta a tela, garantindo que o CCO nunca perca a visão da planta por acidente.
## 4.2. O Painel Principal (MainWindow.xaml.cs)
Esta é a tela principal com as abas: Rotas Ativas, Fila de Rotas e Logs.
* **O Coração Pulsante (DispatcherTimer):**
Como o porto está sempre em movimento, a tela não pode ser estática. No construtor da tela, existe um DispatcherTimer configurado para "bater" a cada 10 segundos (10000 milissegundos).
A cada batida, ele chama as funções filter_active() e filter_queue() para ir ao banco de dados e atualizar as listas de rotas.
* **O Problema do "Congelamento de Tela" (Dispatcher.BeginInvoke):**
Se o sistema fosse ao banco de dados ler milhares de equipamentos usando a mesma thread (linha de execução) que desenha os botões na tela, a tela iria "congelar" e o Windows mostraria a mensagem "Não Respondendo".
Para evitar isso, toda comunicação com o banco é enviada para o Background (segundo plano) usando o código:
code
C#
System.Windows.Application.Current.Dispatcher.BeginInvoke(
    DispatcherPriority.Background, new Action(() => { filter_active(); })
);
Isso faz com que a rodinha de carregamento (pbFooter) gire suavemente enquanto o banco trabalha.
* **A Comunicação entre Telas (Classe WComm.cs):**
O WComm atua como um "mensageiro" (ou carteiro) entre a tela principal e a tela de adicionar rotas. Ele carrega informações vitais na memória, como o ID da rota selecionada (routeId), a mensagem de feedback para o usuário (message) e se o operador está tentando substituir uma rota falha (replace = true).
## 4.3. Adicionando e Substituindo Rotas (Add.xaml.cs)
Quando o operador clica em "Adicionar" ou "Substituir", uma janela pop-up menor se abre.
* **Filtros Inteligentes (filter e RadioButton_Checked):**
O método filter() é o cérebro da busca. Ele pega todas as milhares de rotas possíveis e vai subtraindo o que não serve:
Tira as rotas que já estão ativas.
Tira as rotas que já estão na fila.
Baseado no "Radio Button" clicado (ex: Píer 1 ou Píer 2), ele lê o arquivo de configuração App.config e remove os destinos indesejados.
Se o usuário digitar um equipamento no campo de texto (TxtAsset1), ele filtra apenas rotas que contêm aquela palavra.
* **Troca Quente / Hot-Swap (A Lógica do replace):**
Se uma correia quebrar, o navio não pode parar de carregar. O operador clica em "Substituir". A variável wcomm.replace avisa a tela de Add para não mostrar todas as rotas, mas apenas rotas alternativas que desaguem no mesmo destino da rota quebrada, acionando a function fn_RouteReplacer_I_Route_O_Route no banco.
* **Como a Tela Filha avisa a Tela Mãe? (WindowHiddenCallback):**
Quando o operador escolhe a rota e clica em "Salvar", a janela Add faz um INSERT na tabela rRouteQueue e se esconde (this.Hide()).
Neste exato momento, ela aciona o "botão de pânico" chamado WindowHiddenCallback?.Invoke(). Esse comando viaja até a MainWindow e dispara a função TempWindow_Hidden, avisando: "Terminei, pode atualizar sua tabela principal!"
## 4.4. A Prova de Segurança (Consistency.xaml.cs)
Ter uma rota na fila não significa que ela pode ser ligada. É obrigatório passar pelo teste de segurança.
* **O Check-up Físico:**
O método filter() desta janela consulta as "Views Mágicas" do banco de dados (ex: vw_Route_Consistency_Tripper).
* **Traduzindo para o Operador:**
Se a view do banco disser que a Tag física do motor está diferente da Tag desejada pelo sistema (BoolVeredict == 0), o C# empacota esse erro em uma lista amigável:
code
C#
Tipo = "Posição de cabeça móvel", Status = "Inconsistente"
Isso é jogado no DataGrid (gdConsistency), dando ao operador o diagnóstico exato de qual máquina no pátio o eletricista precisa ir consertar antes de a rota poder ser ativada.
