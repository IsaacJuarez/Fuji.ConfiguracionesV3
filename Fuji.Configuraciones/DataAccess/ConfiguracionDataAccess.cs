using Fuji.Configuraciones.Entidades;
using Fuji.Configuraciones.Extensions;
using Fuji.Configuraciones.Feed2Service;
using System;
using System.Linq;

namespace Fuji.Configuraciones.DataAccess
{
    public class ConfiguracionDataAccess
    {
        //public dbConfigEntities ConfigDA;
        public static NapoleonServiceClient NapoleonDA = new NapoleonServiceClient();

        #region gets
        public ClienteF2CResponse getConfiguracion(string vchClaveSitio, int id_Sitio, ref string mensaje)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            ClienteF2CRequest request = new ClienteF2CRequest();
            try
            {
                request.vchClaveSitio = vchClaveSitio;
                request.Token = Security.Encrypt(id_Sitio + "|" + vchClaveSitio);
                request.id_Sitio = id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = vchClaveSitio;
                response = NapoleonDA.getConeccion(request);
                mensaje = response.message;
                response.valido = true;
            }
            catch(Exception egc)
            {
                Log.EscribeLog("Existe un error al intentar obtener los datos de configuración del Sitio: " + egc.Message);
                response = null;
                response.valido = false;
                mensaje = egc.Message;
            }
            return response;
        }

        public ClienteF2CResponse getVerificaSitio(string vchClaveSitio,int id_Sitio, ref string mensaje)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            ClienteF2CRequest request = new ClienteF2CRequest();
            try
            {
                request.vchClaveSitio = vchClaveSitio;
                request.Token = Security.Encrypt(id_Sitio + "|" + vchClaveSitio);
                request.id_Sitio = id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = vchClaveSitio;
                response = NapoleonDA.getVerificaSitio(request);
                mensaje = response.message;
                response.valido = true;
            }
            catch (Exception egV)
            {
                response.message = egV.Message;
                response.valido = false;
                Log.EscribeLog("Existe un error en ConfiguracionDataAccess.getVerificaSitio: " + egV.Message);
            }
            return response;
        }
        #endregion gets

        #region sets
        public ClienteF2CResponse setConfiguracion(Feed2Service.tbl_ConfigSitio mdlConfig, ref string mensaje,ref int id_Sitio)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            ClienteF2CRequest request = new ClienteF2CRequest();
            try
            {
                request.mdlConfig = mdlConfig;
                request.id_Sitio = mdlConfig.id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = mdlConfig.vchClaveSitio;
                request.Token = Security.Encrypt(mdlConfig.id_Sitio + "|" + mdlConfig.vchClaveSitio);
                response = NapoleonDA.setConfiguracion(request);
                mensaje = response.message;
                id_Sitio = response.id_Sitio;
                response.valido = true;
            }
            catch(Exception esC)
            {
                Log.EscribeLog("Existe un error al guardar la información" + esC.Message);
                response.valido = false;
                mensaje = esC.Message;
            }
            return response;
        }

        public ClienteF2CResponse updateConfiguracion(Feed2Service.clsConfiguracion mdlConfig, ref string mensaje)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            ClienteF2CRequest request = new ClienteF2CRequest();
            try
            {
                request.mdlConfiguracion = mdlConfig;
                request.id_Sitio = mdlConfig.id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = mdlConfig.vchClaveSitio;
                request.Token = Security.Encrypt(mdlConfig.id_Sitio + "|" + mdlConfig.vchClaveSitio);
                response = NapoleonDA.updateConfiguracion(request);
                mensaje = response.message;
            }
            catch (Exception esC)
            {
                Log.EscribeLog("Existe un error al guardar la información: " + esC.Message);
                response.valido = false;
                mensaje = esC.Message;
            }
            return response;
        }

        public ClienteF2CResponse updateConfiguracionServer(Feed2Service.clsConfiguracion mdlConfig, ref string mensaje)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            ClienteF2CRequest request = new ClienteF2CRequest();
            try
            {
                request.mdlConfiguracion = mdlConfig;
                request.Token = Security.Encrypt(mdlConfig.id_Sitio + "|" + mdlConfig.vchClaveSitio);
                request.id_Sitio = mdlConfig.id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = mdlConfig.vchClaveSitio;
                response = NapoleonDA.updateConfiguracionServer(request);
                mensaje = response.message;
            }
            catch (Exception esC)
            {
                Log.EscribeLog("Existe un error al actualizar la información: " + esC.Message);
                response.valido = false;
                mensaje = esC.Message;
            }
            return response;
        }

        public ClienteF2CResponse getXMLFileConfig(string vchClave, int id_Sitio, string vchClaveSitio)
        {
            ClienteF2CResponse response = new ClienteF2CResponse();
            ClienteF2CRequest request = new ClienteF2CRequest();
            try
            {
                request.vchPassword = vchClave;
                request.id_Sitio = id_Sitio;
                //request.id_SitioSpecified = true;
                request.vchClaveSitio = vchClaveSitio;
                request.Token = Security.Encrypt(id_Sitio + "|" + vchClaveSitio);
                response = NapoleonDA.getXMLFileConfig(request);
            }
            catch(Exception eXML)
            {
                response.vchFormato = "";
                Log.EscribeLog("Existe un error al obtener el formato XML: " + eXML.Message);
            }
            return response;
        }
        #endregion sets
    }
}
