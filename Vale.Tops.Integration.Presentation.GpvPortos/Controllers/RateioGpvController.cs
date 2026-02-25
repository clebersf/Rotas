using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;

namespace Vale.Tops.Integration.Presentation.GpvPortos.Controllers
{
    public class RateioGpvController : ApiController
    {
        private IvwRateioGpvPublishReadOnly vwRateioGpvPublishReadOnly;
        private IvwRateioGpvDescPublishReadOnly vwRateioGpvDescPublishReadOnly;
        private IvwRateioGpvAllPublishReadOnly vwRateioGpvAllPublishReadOnly;
        /// <summary>
        /// Integração do sistema de rateio do porto tu com o sistema GPV portos. | 
        /// Parâmetro de entrada: Data inicial e Data Final para comparar a com o campo data inicial do passo. | 
        /// Retorno: Lista de informações do Rateio dos passos do navio
        /// do ponto de vista da origem. O RETORNO SERÁ LIMITADO A UM HISTÓRICO DE DO MAXIMO 7 DIAS DE INFORMAÇÃO.
        /// </summary>
        /// <param name="dhi"> Data inicial para a comparação com o campo "PassoDhInicio" relativo a data inicial do passo. Formato: mm/dd/yyyy (mes/dia/ano).</param>
        /// <param name="dhf"> Data final para a comparação com o campo "PassoDhInicio" relativo a data inicial do passo. Formato: mm/dd/yyyy (mes/dia/ano).</param>
        /// <returns>Lista de informações do Rateio dos passos do navio do ponto de vista da origem.
        /// </returns>
        [HttpGet]
        [Route("Tu_RateioGpv_I_Dhi_Dhf_O_RateioList")]
        [ResponseType(typeof(vwRateioGpvPublish))]
        public async Task<HttpResponseMessage> Get (DateTime dhi, DateTime dhf)
        {
            // Limita retorno a ultimos 7 dias
            DateTime dhfcalc = (dhf - dhi).Days > 7 ? dhi.AddDays(7) : dhf;
            ILog log = LogManager.GetLogger("Tu_RateioGpv_I_Dhi_Dhf_O_OrigemList");
            try
            {            
                this.vwRateioGpvPublishReadOnly = new vwRateioGpvPublishReadOnly();
                IList<vwRateioGpvPublish> result = await Task.Run(() => vwRateioGpvPublishReadOnly.All().Where(p => p.PassoDhInicio >= dhi & p.PassoDhInicio <= dhfcalc).ToList());
                var response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Erro ao obter o relatório de rateio para gpvportos com parâmentros: ini=[{0}] end=[{1}].",
                                        "dhi.ToString()", "dhf.ToString())", ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Integração do sistema de rateio do porto tu com o sistema GPV portos (Para o Processo da Descarga). | 
        /// Parâmetro de entrada: Data inicial e Data Final para comparar a com o campo data inicial do passo. | 
        /// Retorno: Lista de informações do Rateio da descarga
        /// do ponto de vista da origem. O RETORNO SERÁ LIMITADO A UM HISTÓRICO DE DO MAXIMO 7 DIAS DE INFORMAÇÃO.
        /// </summary>
        /// <param name="dhi"> Data inicial para a comparação com o campo "PassoDhInicio" relativo a data inicial do passo. Formato: mm/dd/yyyy (mes/dia/ano).</param>
        /// <param name="dhf"> Data final para a comparação com o campo "PassoDhInicio" relativo a data inicial do passo. Formato: mm/dd/yyyy (mes/dia/ano).</param>
        /// <returns>Lista de informações do Rateio dos passos da descarga do ponto de vista da origem.
        /// </returns>
        [HttpGet]
        [Route("Tu_RateioGpvDesc_I_Dhi_Dhf_O_RateioList")]
        [ResponseType(typeof(vwRateioGpvDescPublish))]
        public async Task<HttpResponseMessage> DescGet(DateTime dhi, DateTime dhf)
        {
            // Limita retorno a ultimos 7 dias
            DateTime dhfcalc = (dhf - dhi).Days > 7 ? dhi.AddDays(7) : dhf;
            ILog log = LogManager.GetLogger("Tu_RateioGpvDesc_I_Dhi_Dhf_O_OrigemList");
            try
            {
                this.vwRateioGpvDescPublishReadOnly = new vwRateioGpvDescPublishReadOnly();
                IList<vwRateioGpvDescPublish> result = await Task.Run(() => vwRateioGpvDescPublishReadOnly.All().Where(p => p.PassoDhInicio >= dhi & p.PassoDhInicio <= dhfcalc.AddSeconds(86400)).ToList());
                var response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Erro ao obter o relatório de rateio para gpvportos com parâmentros: ini=[{0}] end=[{1}].",
                                        "dhi.ToString()", "dhf.ToString())", ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Integração do sistema de rateio do porto tu com o sistema GPV portos (Para o Processo da Descarga). | 
        /// Parâmetro de entrada: Data inicial e Data Final para comparar a com o campo data inicial do passo. | 
        /// Retorno: Lista de informações do Rateio da descarga
        /// do ponto de vista da origem. O RETORNO SERÁ LIMITADO A UM HISTÓRICO DE DO MAXIMO 7 DIAS DE INFORMAÇÃO.
        /// </summary>
        /// <param name="dhi"> Data inicial para a comparação com o campo "PassoDhInicio" relativo a data inicial do passo. Formato: mm/dd/yyyy (mes/dia/ano).</param>
        /// <param name="dhf"> Data final para a comparação com o campo "PassoDhInicio" relativo a data inicial do passo. Formato: mm/dd/yyyy (mes/dia/ano).</param>
        /// <returns>Lista de informações do Rateio dos passos da descarga do ponto de vista da origem.
        /// </returns>
        [HttpGet]
        [Route("Tu_RateioGpvAll_I_Dhi_Dhf_O_RateioList")]
        [ResponseType(typeof(vwRateioGpvAllPublish))]
        public async Task<HttpResponseMessage> AllGet(DateTime dhi, DateTime dhf)
        {
            // Limita retorno a ultimos 7 dias
            DateTime dhfcalc = (dhf - dhi).Days > 7 ? dhi.AddDays(7) : dhf;
            ILog log = LogManager.GetLogger("Tu_RateioGpvAll_I_Dhi_Dhf_O_OrigemList");
            try
            {
                this.vwRateioGpvAllPublishReadOnly = new vwRateioGpvAllPublishReadOnly();
                IList<vwRateioGpvAllPublish> result = await Task.Run(() => vwRateioGpvAllPublishReadOnly.All().Where(p => p.PassoDhInicio >= dhi & p.PassoDhInicio <= dhfcalc.AddSeconds(86400)).ToList());
                var response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Erro ao obter o relatório de rateio para gpvportos com parâmentros: ini=[{0}] end=[{1}].",
                                        "dhi.ToString()", "dhf.ToString())", ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
