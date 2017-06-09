using Fuji.Configuraciones.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuji.Configuraciones.DataAccess
{
    public class LoginDataAccess
    {
        public dbConfigEntities UserDA;


        #region gets
        public bool AgregarUsuario(tbl_CAT_Usuarios mdlUser, ref string OutMessage)
        {
            bool Exito = false;
            try
            {
                UserDA = new dbConfigEntities();
                var users = (from registro in UserDA.tbl_CAT_Usuarios
                               where registro.vchUsuario == mdlUser.vchUsuario
                             select registro).ToList();
                if (users.Count > 0)
                {
                    OutMessage = "El usuario que deseas dar de alta ya está registrado";
                    Exito = false;
                }
                else
                {
                    tbl_CAT_Usuarios _usuario = new tbl_CAT_Usuarios();
                    _usuario.vchNombre = mdlUser.vchNombre;
                    _usuario.intTipoUsuarioID = mdlUser.intTipoUsuarioID;
                    _usuario.vchPassword = mdlUser.vchPassword;
                    _usuario.vchUsuario = mdlUser.vchUsuario;
                    _usuario.vchUserAdmin = mdlUser.vchUserAdmin;
                    _usuario.datFecha = DateTime.Today;
                    _usuario.bitActivo = true;
                    UserDA.tbl_CAT_Usuarios.Add(_usuario);
                    UserDA.SaveChanges();
                    OutMessage = "Se agregó correctamente el usuario";
                    Exito = true;
                }
                UserDA.Dispose();
            }
            catch (Exception exc)
            {
                OutMessage = "Error en el sistema: " + exc.Message;
                Log.EscribeLog("Error en UsuariosController.AagregarUsuario(" + mdlUser.vchNombre + "): " + exc.Message);
            }
            return Exito;
        }

        /// <summary>
        /// Permite acutalizar la información del usuario.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="nombre"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="usuario"></param>
        /// <param name="OutMessage"></param>
        /// <returns></returns>
        public bool ActualizarUsuario(tbl_CAT_Usuarios user, ref string OutMessage)
        {
            bool Actualizado = false;
            try
            {
                UserDA = new dbConfigEntities();
                tbl_CAT_Usuarios _usuario = UserDA.tbl_CAT_Usuarios.First(i => i.intUsuarioID == user.intUsuarioID);
                _usuario.vchNombre = user.vchNombre;
                _usuario.vchUsuario = user.vchUsuario;
                _usuario.vchPassword = user.vchPassword;
                _usuario.vchUserAdmin = user.vchUserAdmin;
                _usuario.datFecha = DateTime.Today;
                _usuario.bitActivo = true;
                UserDA.SaveChanges();
                OutMessage = "La actualización se ha efectuado correctamente";
                Actualizado = true;

                UserDA.Dispose();
            }
            catch (Exception exc)
            {
                OutMessage = "Error en el sistema: " + exc.Message;
                Log.EscribeLog("Error en LoginDataAccess.ActualizarUsuario(" + user.intUsuarioID + "): " + exc.Message);
            }
            return Actualizado;
        }

        /// <summary>
        /// Permite activar o desactivar el usuario.
        /// </summary>
        /// <param name="User"></param>
        /// <param name="bitActivo"></param>
        /// <param name="OutMessage"></param>
        /// <returns></returns>
        public bool ActivarUsuario(int intUsuarioID, bool bitActivo, ref string OutMessage)
        {
            bool Actualizado = false;
            try
            {
                UserDA = new dbConfigEntities();
                tbl_CAT_Usuarios _usuario = UserDA.tbl_CAT_Usuarios.First(i => i.intUsuarioID == intUsuarioID);
                _usuario.bitActivo = bitActivo;
                _usuario.datFecha = DateTime.Today;
                UserDA.SaveChanges();
                OutMessage = "La actualización se ha efectuado correctamente";
                Actualizado = true;
                UserDA.Dispose();
            }
            catch (Exception exc)
            {
                OutMessage = "Error en el sistema: " + exc.Message;
                Log.EscribeLog("Error en LoginDataAccess.ActivarUsuario(" + intUsuarioID + "): " + exc.Message);
            }
            return Actualizado;
        }
        #endregion gets

        #region sets
        /// <summary>
        /// Permite logear a un usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="User"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public bool Logear(string user, string password, ref tbl_CAT_Usuarios User, ref string outMessage)
        {
            bool Success = false;
            try
            {
                using (UserDA = new dbConfigEntities())
                {
                    if (UserDA.tbl_CAT_Usuarios.Any(u => u.vchUsuario.ToUpper().Trim() == user.ToUpper().Trim() && u.vchPassword == password && (bool)u.bitActivo))
                    {
                        var _usr = (from em in UserDA.tbl_CAT_Usuarios
                                    where em.bitActivo == true && em.vchUsuario == user && em.vchPassword == password && (bool)em.bitActivo
                                    select new
                                    {
                                        Id = em.intUsuarioID,
                                        Nombre = em.vchNombre ?? "",
                                        _pass = em.vchPassword ?? "",
                                        bitActivo = em.bitActivo,
                                        usuario = em.vchUsuario,
                                        _tipoUsuario = em.intTipoUsuarioID
                                    }).First();
                        if (Success = _usr.Id > 0 ? true : false)
                        {
                            User.intUsuarioID = _usr.Id;
                            User.vchNombre = _usr.Nombre;
                            User.vchPassword = _usr._pass;
                            User.bitActivo = _usr.bitActivo;
                            User.vchUsuario = _usr.usuario;
                            User.intTipoUsuarioID = _usr._tipoUsuario;
                        }
                    }
                    else
                    {
                        outMessage = "Contraseña o usuario incorrecta";
                        Success = false;
                    }
                }
            }
            catch (Exception exc)
            {
                Log.EscribeLog("Error en LoginDataAccess.Logear: " + exc.Message);
            }
            return Success;
        }

        public List<tbl_CAT_Usuarios> getUsuarios()
        {
            List<tbl_CAT_Usuarios> _lstUserRes = new List<tbl_CAT_Usuarios>();
            try
            {
                using(UserDA = new dbConfigEntities())
                {
                    var users = from item in UserDA.tbl_CAT_Usuarios
                                select item;
                    if(users != null)
                    {
                        if(users.Count() > 0)
                        {
                            _lstUserRes.AddRange(users);
                        }
                    }
                }
            }
            catch(Exception egU)
            {
                Log.EscribeLog("Existe un error en LoginDataAccess.getUsuarios: " + egU.Message);
            }
            return _lstUserRes;
        }

        public List<tbl_ConfigSitio> getSitios()
        {
            List<tbl_ConfigSitio> _lstSitios = new List<tbl_ConfigSitio>();
            try
            {
                using (UserDA = new dbConfigEntities())
                {
                    var sitios = from item in UserDA.tbl_ConfigSitio
                                select item;
                    if (sitios != null)
                    {
                        if (sitios.Count() > 0)
                        {
                            _lstSitios.AddRange(sitios);
                        }
                    }
                }
            }
            catch (Exception egS)
            {
                Log.EscribeLog("Existe un error en LoginDataAccess.getSitios: " + egS.Message);
            }
            return _lstSitios;
        }
        #endregion sets
    }
}
