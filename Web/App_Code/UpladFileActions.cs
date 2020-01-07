// --------------------------------
// <copyright file="UpladFileActions.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using AspadLandFramework.Item;
using SbrinnaCoreFramework.Activity;

/// <summary>
/// Summary description for UpladFileActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class UpladFileActions : WebService
{
    public UpladFileActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long attachId, int companyId)
    {
        return UploadFile.Delete(attachId, companyId);
    }
}