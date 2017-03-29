using bringpro.Data;
using bringpro.Web.Domain;
using bringpro.Web.Enums;
using bringpro.Web.Models.Requests;
using bringpro.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace bringpro.Web.Services
{
    public class WebsiteSettingsServices : BaseService
    {

        public static List<WebsiteSettings> GetWebsiteSettingsBySlug(int WebsiteId, List<string> Slugs)
        {
            List<WebsiteSettings> list = new List<WebsiteSettings>();


            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.WebsiteSettings_GetSettingsBySlug"
                  , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@WebsiteId", WebsiteId);

                      SqlParameter s = new SqlParameter("@Slug", SqlDbType.Structured);
                      if (Slugs != null && Slugs.Any())
                      {
                          s.Value = new NVarcharTable(Slugs);
                      }
                      paramCollection.Add(s);

                  }, map: delegate (IDataReader reader, short set)
                  {
                      WebsiteSettings ws = new WebsiteSettings();

                      int startingIndex = 0;
                      ws.Id = reader.GetSafeInt32(startingIndex++);
                      ws.SettingsId = reader.GetSafeInt32(startingIndex++);
                      ws.WebsiteId = reader.GetSafeInt32(startingIndex++);
                      ws.SettingsValue = reader.GetSafeString(startingIndex++);
                      ws.UserId = reader.GetSafeString(startingIndex++);
                      ws.MediaId = reader.GetSafeInt32(startingIndex++);
                      ws.DateAdded = reader.GetSafeDateTime(startingIndex++);
                      ws.DateModified = reader.GetSafeDateTime(startingIndex++);

                      Website w = new Website();

                      w.Id = reader.GetSafeInt32(startingIndex++);
                      w.Name = reader.GetSafeString(startingIndex++);
                      w.Slug = reader.GetSafeString(startingIndex++);
                      w.Description = reader.GetSafeString(startingIndex++);
                      w.Url = reader.GetSafeString(startingIndex++);
                      w.MediaId = reader.GetSafeInt32(startingIndex++);
                      w.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      w.DateModified = reader.GetSafeDateTime(startingIndex++);

                      ws.Website = w;

                      Settings s = new Settings();

                      s.Id = reader.GetSafeInt32(startingIndex++);
                      s.Category = reader.GetSafeEnum<SettingsCategory>(startingIndex++);
                      s.Name = reader.GetSafeString(startingIndex++);
                      s.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      s.DateModified = reader.GetSafeDateTime(startingIndex++);
                      s.SettingType = reader.GetSafeEnum<SettingsType>(startingIndex++);
                      s.Description = reader.GetSafeString(startingIndex++);
                      s.SettingSlug = reader.GetSafeString(startingIndex++);
                      s.SettingSection = reader.GetSafeEnum<SettingsSection>(startingIndex++);

                      ws.Setting = s;

                      Media m = new Media();
                      m.Id = reader.GetSafeInt32(startingIndex++);
                      m.Url = reader.GetSafeString(startingIndex++);
                      m.MediaType = reader.GetSafeInt32(startingIndex++);
                      m.UserId = reader.GetSafeString(startingIndex++);
                      m.Title = reader.GetSafeString(startingIndex++);
                      m.Description = reader.GetSafeString(startingIndex++);
                      m.ExternalMediaId = reader.GetSafeInt32(startingIndex++);
                      m.FileType = reader.GetSafeString(startingIndex++);
                      m.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      m.DateModified = reader.GetSafeDateTime(startingIndex++);

                      if (m.Id != 0)
                      {
                          ws.Media = m;
                      }

                      if (list == null)
                      {
                          list = new List<WebsiteSettings>();
                      }

                      list.Add(ws);
                  }
                  );

                return list;
            }
        }

        public static Dictionary<string, WebsiteSettings> getWebsiteSettingsDictionaryBySlug(int websiteId, List<string> Slugs)
        {
            Dictionary<string, WebsiteSettings> dict = null;
            List<WebsiteSettings> list = GetWebsiteSettingsBySlug(websiteId, Slugs);

            if (list != null)
            {
                dict = new Dictionary<string, WebsiteSettings>();

                foreach (var setting in list)
                {
                    dict.Add(setting.Setting.SettingSlug, setting);
                    
                }
            }

            return dict;

        }        
    }

}