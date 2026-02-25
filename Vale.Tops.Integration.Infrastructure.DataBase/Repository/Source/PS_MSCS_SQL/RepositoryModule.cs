using Autofac;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;

// Garanta que esta classe esteja acessível (public)
public class RepositoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // 1. REGISTRO GENÉRICO CRUCIAL para IWriteRead<T>
        // Resolve: IWriteRead<rRouteActive>, IWriteRead<rRouteQueue>, IWriteRead<Location>
        builder.RegisterGeneric(typeof(Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager.WriteRead<>))
               .As(typeof(IWriteRead<>))
               .InstancePerLifetimeScope(); // Use InstancePerLifetimeScope para Web

        // 2. REGISTRO GENÉRICO para IReadOnly<T>
        // Necessário pois a classe vw_Route_Consistency_ReadOnly herda de ReadOnly<T>
        builder.RegisterGeneric(typeof(Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager.ReadOnly<>))
               .As(typeof(IReadOnly<>))
               .InstancePerLifetimeScope();

        // 3. REGISTRO ESPECÍFICO para Ivw_Route_Consistency_ReadOnly
        // Resolve a primeira dependência específica do seu controller
        builder.RegisterType<vw_Route_Consistency_ReadOnly>()
               .As<Ivw_Route_Consistency_ReadOnly>()
               .InstancePerLifetimeScope();

        // 1. Damper (FALHOU NA RESOLUÇÃO)
        builder.RegisterType<vw_Route_Consistency_Damper_ReadOnly>()
               .As<Ivw_Route_Consistency_Damper_ReadOnly>()
               .InstancePerRequest();

        // 2. Feeder
        builder.RegisterType<vw_Route_Consistency_Feeder_ReadOnly>()
               .As<Ivw_Route_Consistency_Feeder_ReadOnly>()
               .InstancePerRequest();

        // 3. Reversal
        builder.RegisterType<vw_Route_Consistency_Reversal_ReadOnly>()
               .As<Ivw_Route_Consistency_Reversal_ReadOnly>()
               .InstancePerRequest();

        // 4. Rule
        builder.RegisterType<vw_Route_Consistency_Rule_ReadOnly>()
               .As<Ivw_Route_Consistency_Rule_ReadOnly>()
               .InstancePerRequest();

        // 5. Tripper
        builder.RegisterType<vw_Route_Consistency_Tripper_ReadOnly>()
               .As<Ivw_Route_Consistency_Tripper_ReadOnly>()
               .InstancePerRequest();

        // Exemplo de registro no RepositoryModule (usando Autofac)

        builder.RegisterType<vw_Route_Tag_Damper_Permission_ReadOnly>().As<Ivw_Route_Tag_Damper_Permission_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Feeder_Permission_ReadOnly>().As<Ivw_Route_Tag_Feeder_Permission_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Reversal_Permission_ReadOnly>().As<Ivw_Route_Tag_Reversal_Permission_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Tripper_Permission_ReadOnly>().As<Ivw_Route_Tag_Tripper_Permission_ReadOnly>().InstancePerLifetimeScope();

        builder.RegisterType<vw_Route_Tag_Eqp_D_Permission_ReadOnly>().As<Ivw_Route_Tag_Eqp_D_Permission_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Eqp_O_Permission_ReadOnly>().As<Ivw_Route_Tag_Eqp_O_Permission_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_OxD_Permission_ReadOnly>().As<Ivw_Route_Tag_OxD_Permission_ReadOnly>().InstancePerLifetimeScope();

        builder.RegisterType<vw_Route_Tag_Damper_Command_ReadOnly>().As<Ivw_Route_Tag_Damper_Command_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Feeder_Command_ReadOnly>().As<Ivw_Route_Tag_Feeder_Command_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Reversal_Command_ReadOnly>().As<Ivw_Route_Tag_Reversal_Command_ReadOnly>().InstancePerLifetimeScope();
        builder.RegisterType<vw_Route_Tag_Tripper_Command_ReadOnly>().As<Ivw_Route_Tag_Tripper_Command_ReadOnly>().InstancePerLifetimeScope();
        // ... e assim por diante

        // Se você tiver repositórios específicos que não usam o genérico WriteRead/ReadOnly,
        // registre-os explicitamente aqui. Exemplo:
        // builder.RegisterType<rRouteActiveWriteRead>().As<IrRouteActiveWriteRead>().InstancePerLifetimeScope();
    }
}