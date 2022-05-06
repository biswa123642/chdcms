using HtmlAgilityPack;
using RestSharp;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using Convert = System.Convert;

namespace Foundation.Utility.Utilities
{
    public static class BasicUtil
    {
        public static OfficeDetails OfficeDetails = new OfficeDetails();
        public static List<MenuItem> LstMenu = new List<MenuItem>();

        /// <summary>
        /// Function used to remove HTML snippet
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string RemoveHtml(string strSource)
        {
            string result = string.Empty;
            try
            {
                result = !string.IsNullOrEmpty(strSource) ?
               Regex.Replace(strSource, "<(.|\n)*?>", "").Replace("&nbsp;", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Trim() : string.Empty;

            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("RemoveHtml > HoganLovells.CommonLibrary > strSource: " + strSource, exception, typeof(BasicUtil));
            }
            return result;
        }

        /// <summary>
        /// Get Labels From Solr
        /// </summary>
        /// <param name="dictionaryKey"></param>
        /// <param name="isEditable"> </param>
        /// <param name="isMasterIndex"></param>
        /// <returns></returns>
        public static string GetDictionary(string dictionaryKey, bool isEditable = false, bool isMasterIndex = false)
        {
            List<Item> listSearch = new List<Item>();
            string phrase = string.Empty;
            try
            {
                EDSearch.EDSearch eDSearchObject = new EDSearch.EDSearch();

                string searchQuery = "dictionarysearchkey|" + dictionaryKey + ";" + SolrSearchFieldsConstant.Language + "|" + SitecoreUtil.GetContextLanguage();
                listSearch.AddRange(eDSearchObject.SearchResult(searchQuery, null, 1, false, isMasterIndex));
                if (!listSearch.Any())
                {
                    searchQuery = "dictionarysearchkey|" + dictionaryKey + ";" + SolrSearchFieldsConstant.Language + "|" + "en";
                    listSearch.AddRange(eDSearchObject.SearchResult(searchQuery, null, 1, false, isMasterIndex));
                }
                if (listSearch.Count > 0)
                {
                    Item dictionaryItem = listSearch[0];
                    if (dictionaryItem != null)
                    {
                        DictionaryItemItem dictItem = new DictionaryItemItem(dictionaryItem);
                        if (!string.IsNullOrEmpty(dictItem.Phrase))
                            phrase = isEditable ? FieldRenderer.Render(dictionaryItem, DictionaryItemItem.FieldNames.PhraseFieldName) : dictItem.Phrase;
                    }
                }
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("GetDictionary > HoganLovells.CommonLibrary > for Key " + dictionaryKey, exception,
                                               "Dictionary");
            }
            return phrase;
        }

        /// <summary>
        ///  Get Labels From Solr with new parameter of language 
        ///  Method used to get dictionary items language-wise on bioportal main page of login
        /// </summary>
        /// <param name="dictionaryKey"></param>
        /// <param name="Lang"></param>
        /// <param name="isEditable"></param>
        /// <param name="isMasterIndex"></param>
        /// <returns></returns>

        public static string GetBioPortalMainPageDictionary(string dictionaryKey, string Lang, bool isEditable = false, bool isMasterIndex = false)
        {
            List<Item> listSearch = new List<Item>();
            string phrase = string.Empty;
            try
            {
                EDSearch.EDSearch eDSearchObject = new EDSearch.EDSearch();

                string searchQuery = "dictionarysearchkey|" + dictionaryKey + ";" + SolrSearchFieldsConstant.Language + "|" + Lang;
                listSearch.AddRange(eDSearchObject.SearchResult(searchQuery, null, 1, false, isMasterIndex));
                if (!listSearch.Any())
                {
                    searchQuery = "dictionarysearchkey|" + dictionaryKey + ";" + SolrSearchFieldsConstant.Language + "|" + "en";
                    listSearch.AddRange(eDSearchObject.SearchResult(searchQuery, null, 1, false, isMasterIndex));
                }
                if (listSearch.Count > 0)
                {
                    Item dictionaryItem = listSearch[0];
                    if (dictionaryItem != null)
                    {
                        DictionaryItemItem dictItem = new DictionaryItemItem(dictionaryItem);
                        if (!string.IsNullOrEmpty(dictItem.Phrase))
                            phrase = isEditable ? FieldRenderer.Render(dictionaryItem, DictionaryItemItem.FieldNames.PhraseFieldName) : dictItem.Phrase;
                    }
                }
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("GetDictionary > HoganLovells.CommonLibrary > for Key " + dictionaryKey, exception,
                                               "Dictionary");
            }
            return phrase;
        }
        /// <summary>
        /// Sending Email
        /// </summary>
        /// <param name="fromEmailId">From Email id</param>
        /// <param name="toEmailId">To Email ID</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="mailBody">Email Body</param>
        /// <param name="fromDisplayName">From Display name, default is NULL </param>
        /// <param name="contentid"> </param>
        /// <param name="isBodyHtml">whether the email is html, default is true</param>
        /// <param name="logopath"> </param>
        /// <returns></returns>
        public static bool SendEmail(string fromEmailId, string toEmailId, string subject, string mailBody, string fromDisplayName = null, string logopath = "", string contentid = "", bool isBodyHtml = true)
        {
            bool result;
            try
            {
                string hostName = Settings.MailServer;
                int portNumber = Settings.MailServerPort;
                string username = Settings.MailServerUserName;
                string password = Settings.MailServerPassword;
                SmtpClient client = new SmtpClient();
                if (!string.IsNullOrEmpty(hostName))
                {
                    client.Host = hostName;
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    client.Credentials = new NetworkCredential(username, password);
                }
                if (portNumber > 0)
                {
                    client.Port = portNumber;
                }
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                foreach (var address in toEmailId.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address);
                }
                // mail.To.Add(toEmailId);
                if (!string.IsNullOrEmpty(fromEmailId))
                {
                    mail.From = ((!string.IsNullOrEmpty(fromDisplayName)) ? new MailAddress(fromEmailId, fromDisplayName) : new MailAddress(fromEmailId));
                }
                else
                {
                    mail.From = new MailAddress(toEmailId);
                }
                mail.Subject = subject;
                if (!string.IsNullOrWhiteSpace(logopath))
                {
                    mail.AlternateViews.Add(BasicUtil.EmbedCompanyLogo(logopath, mailBody, contentid));
                    ContentType mimeType = new ContentType("text/html");
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(mailBody, mimeType);
                    mail.AlternateViews.Add(alternate);
                }
                else
                {
                    mail.Body = "<html><body>" + mailBody + "</body></html>";
                }

                mail.IsBodyHtml = isBodyHtml;

                if (mail.Body.Contains("cid:"))
                {
                    mail = EmbedImage(mail);
                    mail.Bcc.Add("vidhi.jain@edynamic.net");
                }

                client.Send(mail);


                return true;
            }
            catch (Exception ex)
            {
                //LogManager<ILogProvider>.Error(ex.Message, ex);

                LogManager<ILogProvider>.Error(ex.StackTrace, ex, typeof(BasicUtil));
            }
            result = false;
            return result;
        }

        /// <summary>
        /// Sending Email
        /// </summary>
        /// <param name="fromEmailId">From Email id</param>
        /// <param name="toEmailId">To Email ID</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="mailBody">Email Body</param>
        /// <param name="fromDisplayName">From Display name, default is NULL </param>
        /// <param name="contentid"> </param>
        /// <param name="isBodyHtml">whether the email is html, default is true</param>
        /// <param name="logopath"> </param>
        /// <returns></returns>
        public static bool SendEmailSubscription(string fromEmailId, string toEmailId, string subject, string mailBody, string fromDisplayName = null, string logopath = "", string contentid = "", bool isBodyHtml = true)
        {
            bool result = false;
            try
            {
                string hostName = Settings.MailServer;
                int portNumber = Settings.MailServerPort;
                string username = Settings.MailServerUserName;
                string password = Settings.MailServerPassword;
                SmtpClient client = new SmtpClient();

                if (!string.IsNullOrEmpty(hostName))
                {
                    client.Host = hostName;
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    client.Credentials = new NetworkCredential(username, password);
                }
                if (portNumber > 0)
                {
                    client.Port = portNumber;
                }
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmailId);
                if (!string.IsNullOrEmpty(fromEmailId))
                {
                    mail.From = ((!string.IsNullOrEmpty(fromDisplayName)) ? new MailAddress(fromEmailId, fromDisplayName) : new MailAddress(fromEmailId));
                }
                else
                {
                    mail.From = new MailAddress(toEmailId);
                }
                mail.Subject = subject;

                mailBody = "<html><body>" + mailBody + "</body></html>";

                if (!string.IsNullOrWhiteSpace(logopath))
                {
                    mail.AlternateViews.Add(EmbedCompanyLogoForSubscription(logopath, mailBody, contentid));

                }
                else
                {
                    mail.Body = mailBody;
                }

                mail.IsBodyHtml = isBodyHtml;

                if (mail.Body.Contains("cid:"))
                {
                    mail = EmbedImage(mail);
                    mail.Bcc.Add("vidhi.jain@edynamic.net");
                }

                client.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                //LogManager<ILogProvider>.Error(ex.Message, ex);
                LogManager<ILogProvider>.Error(ex.StackTrace, ex, typeof(BasicUtil));
            }

            return result;
        }

        /// <summary>
        /// Sending Email Via Mailgun Emmail Service
        /// </summary>
        /// <param name="fromEmailId"></param>
        /// <param name="toEmailId"></param>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        /// <param name="topicName"> </param>
        /// <param name="isBodyHtml"></param>
        /// <returns></returns>
        public static string SendEmailMailgun(string fromEmailId, string toEmailId, string subject, string mailBody, string topicName, bool isBodyHtml = true)
        {
            RestClient restClient = new RestClient();
            string sendEmailMailgunAPIKey = Convert.ToString(WebConfigurationManager.AppSettings["SendEmailMailgunAPIKey"]);
            restClient.BaseUrl = "https://api.mailgun.net/v3";
            restClient.Authenticator =
            new HttpBasicAuthenticator("api",
                                      sendEmailMailgunAPIKey);
            RestRequest restRequest = new RestRequest();
            restRequest.AddParameter("domain", "staging.hoganlovells.com", ParameterType.UrlSegment);
            restRequest.Resource = "{domain}/messages";
            restRequest.AddParameter("from", fromEmailId);
            restRequest.AddParameter("to", toEmailId);
            restRequest.AddParameter("subject", subject);
            restRequest.AddParameter("html", mailBody);
            restRequest.AddParameter("o:tag", topicName);
            restRequest.Method = RestSharp.Method.POST;
            return restClient.Execute(restRequest).StatusCode.ToString();


        }

        public static MailMessage EmbedImage(MailMessage mail)
        {
            try
            {
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(mail.Body, null, MediaTypeNames.Text.Html);
                string strImageUrl = HostingEnvironment.MapPath("~/Assets/images/right-arrow.gif");
                LinkedResource logo = new LinkedResource(strImageUrl, MediaTypeNames.Image.Jpeg);
                logo.ContentId = "Pic1";
                av1.LinkedResources.Add(logo);

                string strImageUrl1 = HostingEnvironment.MapPath("~/Assets/images/btn-save-exit.gif");
                LinkedResource logo1 = new LinkedResource(strImageUrl1, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                logo1.ContentId = "Pic2";
                av1.LinkedResources.Add(logo1);

                string strImageUrl2 = HostingEnvironment.MapPath("~/Assets/images/form-area.jpg");
                LinkedResource logo2 = new LinkedResource(strImageUrl2, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                logo2.ContentId = "Pic3";
                av1.LinkedResources.Add(logo2);
                mail.AlternateViews.Add(av1);
            }
            catch (Exception ex)
            {
                LogManager<ILogProvider>.Error(ex.Message, ex);
            }

            return mail;
        }

        /// <summary>
        /// Check whether item is of type folder
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsOfTypeFolder(Item item)
        {
            return item.TemplateID.Equals(AlphaFolderItem.TemplateID)
                || item.TemplateID.Equals(TemplateIDs.Folder)
                || item.TemplateID.Equals(FolderItem.TemplateID)
                || item.TemplateID.Equals(YearFolderItem.TemplateID)
                || item.TemplateID.Equals(MonthFolderItem.TemplateID)
                || item.TemplateID.Equals(EventDayFolderItem.TemplateID)
                || item.TemplateID.Equals(NewsDayFolderItem.TemplateID)
                || item.TemplateID.Equals(PublicationDayFolderItem.TemplateID)
                || item.TemplateID.Equals(SingleBlogDayFolderItem.TemplateID);
        }

        /// <summary>
        /// Check whether item is not of type Thought Leadership (News/Events/Blogs/Publications)
        /// </summary>
        /// <param name="itemTemplateId"> </param>
        /// <returns></returns>
        public static bool NotOfTypeThoughtLeadership(ID itemTemplateId)
        {
            return !itemTemplateId.Equals(PublicationPageItem.TemplateID)
                && !itemTemplateId.Equals(NewsPageItem.TemplateID)
                && !itemTemplateId.Equals(EventPageItem.TemplateID)
                && !itemTemplateId.Equals(SingleBlogItem.TemplateID);
        }

        /// <summary>
        /// Split a Collection Data Vertically in number of columns provided
        /// </summary>
        /// <param name="list"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static dynamic SplitListData(IList<dynamic> list, int column)
        {

            dynamic items = list.Select((i, index)
                => new
                {
                    i,
                    index
                }).GroupBy(group => group.index / (column == 0 ? 1 : column),
                                                                     element => element.i);

            return items;
        }
        /// <summary>
        /// Split a Collection Data horizontly in number of columns provided
        /// </summary>
        /// <param name="list"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static dynamic SplitListDataHorizontally(IList<dynamic> list, int column)
        {
            column = column == 0 ? 1 : column;
            int nearestdivisible = (list.Count % column) == 0 ? list.Count : ((list.Count / column) + 1) * column;

            int divider = list.Count > (column - 1) ? nearestdivisible / column : 1;

            dynamic items = list.Select((i, index)
                => new
                {
                    i,
                    index
                }).GroupBy(group => group.index / divider, element => element.i);

            return items;
        }

        /// <summary>
        /// Get Social Media Item 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<content.SocialMediaItem> GetSocialMedia(Item item, Database db)
        {
            List<content.SocialMediaItem> socialMediaItem = null;

            if (item != null && item.Children != null && item.Children.Any())
            {

                var socialmedia = "Social Media";
                if (socialmedia != null)
                {
                    try
                    {
                        Item socialMediaItemFolder = item.Children.Where(x => x.Name.ToLower().Contains(socialmedia.ToLower())).FirstOrDefault();
                        if (socialMediaItemFolder != null)
                        {
                            var socialMediaRef = socialMediaItemFolder.GetChildren().Where(
                                                           x => x != null && x.TemplateID.ToString().Equals(content.SocialMediaItem.TemplateID.ToString())
                                                           && x.Publishing.HideVersion == false && x.Publishing.NeverPublish == false).ToList();
                            if (socialMediaRef != null && socialMediaRef.Any())
                            {
                                socialMediaItem = new List<content.SocialMediaItem>();
                                socialMediaItem = socialMediaRef.Select(p => (content.SocialMediaItem)p).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("GetSocialMedia > HoganLovells.CommonLibrary > itemID: " + item.ID.ToString(), ex.InnerException,
                                              typeof(BasicUtil));
                    }

                }
            }
            return socialMediaItem;
        }

        /// <summary>
        /// Get Social Media Item Details
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<SocialLink> GetSocialMediaItems(Item item, Database db = null)
        {
            List<SocialLink> socialLinks = new List<SocialLink>();
            List<content.SocialMediaItem> socialMediaItems = new List<content.SocialMediaItem>();

            try
            {
                socialMediaItems = GetSocialMedia(item, db);
                string socialMediaLinkItem = string.Empty;
                if (socialMediaItems != null && socialMediaItems.Any())
                {
                    List<content.SocialMediaItem> socialMediaItem = socialMediaItems.Where(x => x.SocialMediaNetworkItem != null && x.InnerItem.HasLanguageVersion(Context.Language)).ToList();
                    if (socialMediaItem.Any())
                    {
                        foreach (content.SocialMediaItem mediaitem in socialMediaItem)
                        {
                            content.SocialMediaItem socialMedia = Sitecore.Context.Database.GetItem(mediaitem.ID, Sitecore.Context.Language);
                            if (socialMedia != null)
                            {
                                SocialMediaNetworkItem socialitem = new SocialMediaNetworkItem(socialMedia.SocialMediaNetworkItem);
                                var cssClassId = socialitem.InnerItem.Fields["CSSClass"].Value;
                                //RSS internal link item for Main Blog 
                                LinkField socialMediaLink = socialMedia.SocialMediaLink;
                                if (socialMediaLink != null & socialMediaLink.IsInternal && socialMediaLink.TargetItem != null)
                                {
                                    socialMediaLinkItem = socialMediaLink.TargetItem.ID.ToString();

                                }

                                socialLinks.Add(new SocialLink
                                {

                                    Title = socialitem.Name,
                                    Icon = (socialitem.Icon.MediaItem != null ? MediaManager.GetMediaUrl(socialitem.Icon.MediaItem) : ""),
                                    LinkUrl = SitecoreUtil.GetLinkUrl(mediaitem.SocialMediaLink),
                                    Target = "_blank",
                                    SocialMediaItemID = socialitem.ID.ToString(),
                                    TargetItemId = socialMediaLinkItem
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetSocialMediaItems > HoganLovells.CommonLibrary > " + ex.InnerException, typeof(BasicUtil));
            }

            return socialLinks;
        }

        /// <summary>
        /// Get Contact Form ToEmail ID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetContactToEmail(Item item)
        {
            string email = string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                string contactformsetting;
                if (item.TemplateID == HLConstants.TopicCenterBlogFocusedTemplateId || item.TemplateID == HLConstants.TopicCenterNoBlogTemplateId || item.TemplateID == HLConstants.TopicInteractiveTemplateId)
                {
                    contactformsetting = HLConstants.TopicCenterContactFormItemId;
                }
                else
                {
                    contactformsetting = HLConstants.ContactFormSettingItemId;
                }
                ContactFormSettingItem contactFormSettingItem = new ContactFormSettingItem(SitecoreUtil.GetItem(Sitecore.Configuration.Settings.GetSetting(contactformsetting), null));
                email = contactFormSettingItem.DefaultToEmail;
            }
            return email;
        }


        /// <summary>
        /// Get Contact Form ToEmail ID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ContactFormPersonDetail GetContactFormPerson(Item item)
        {
            string email = string.Empty;
            string personName = string.Empty;
            string personRole = string.Empty;
            Item contactPersonItem = ((ContactFormItem)item).ContactPersonItem;
            if (contactPersonItem != null)
            {
                LawyerProfilePageItem contactPerson = ((ContactFormItem)item).ContactPersonItem;
                if (contactPerson != null && contactPerson.InnerItem.TemplateID.Equals(LawyerProfilePageItem.TemplateID))
                {
                    email = contactPerson.BusinessEmail;
                    personName = BioInformation.PersonFullName(contactPerson);
                    personRole = BioInformation.GetBioPositionTitle(contactPerson);
                }
            }
            if (item.TemplateID == NewsPageItem.TemplateID)
            {
                email = ((NewsPageItem)item).ContactEmail;
            }
            else if (item.TemplateID == PublicationPageItem.TemplateID)
            {
                email = ((PublicationPageItem)item).ContactEmail;
            }
            if (string.IsNullOrEmpty(email))
            {
                string contactformsetting;
                if (item.TemplateID == HLConstants.TopicCenterBlogFocusedTemplateId || item.TemplateID == HLConstants.TopicCenterNoBlogTemplateId ||
                    item.TemplateID == HLConstants.TopicInteractiveTemplateId || item.TemplateID == SingleBlogItem.TemplateID ||
                    item.TemplateID == MainBlogViewPageItem.TemplateID || item.TemplateID.ToString().Equals(HLConstants.TopicCenterWithHorizontalMenuTemplateId))
                {
                    email = ((ContactUsEmailItem)item).ContactEmail;
                    contactformsetting = HLConstants.TopicCenterContactFormItemId;
                }
                else if (item.TemplateID == NewsPageItem.TemplateID || item.TemplateID == PublicationPageItem.TemplateID)
                {
                    contactformsetting = HLConstants.NewsPublicationContactFormSettingItemId;
                }
                else if (item.TemplateID.Equals(MainBlogViewPageItem.TemplateID) || item.TemplateID.Equals(SingleBlogItem.TemplateID))
                {
                    email = ((ContactUsEmailItem)item).ContactEmail;
                    contactformsetting = HLConstants.ContactFormSettingItemId;
                }
                else
                {
                    contactformsetting = HLConstants.ContactFormSettingItemId;
                }
                ContactFormSettingItem contactFormSettingItem = new ContactFormSettingItem(SitecoreUtil.GetItem(Sitecore.Configuration.Settings.GetSetting(contactformsetting), null));

                personName = string.Empty;
                personRole = string.Empty;

                if (string.IsNullOrWhiteSpace(email))
                {
                    email = contactFormSettingItem.DefaultToEmail;
                }
            }
            return new ContactFormPersonDetail
            {
                ToEmail = email,
                PersonName = personName,
                PersonRole = personRole
            };
        }

        /// <summary>
        /// Get Contact Form Settings Item Detail
        /// </summary>
        /// <returns></returns>
        public static ContactFormSettingItem GetContactFormSettings(Item item)
        {
            string contactformsetting;
            if (item.TemplateID == HLConstants.TopicCenterBlogFocusedTemplateId ||
                item.TemplateID == HLConstants.TopicCenterNoBlogTemplateId || item.TemplateID == HLConstants.TopicInteractiveTemplateId ||
                item.TemplateID == SingleBlogItem.TemplateID || item.TemplateID == MainBlogViewPageItem.TemplateID ||
                item.TemplateID.ToString().Equals(HLConstants.TopicCenterWithHorizontalMenuTemplateId))
            {
                contactformsetting = HLConstants.TopicCenterContactFormItemId;
            }
            else if (item.TemplateID == NewsPageItem.TemplateID || item.TemplateID == PublicationPageItem.TemplateID)
            {
                contactformsetting = HLConstants.NewsPublicationContactFormSettingItemId;
            }

            else if (item.TemplateID == EventPageItem.TemplateID)
            {
                contactformsetting = "RSVPFormSettingItemId";
            }
            else
            {
                contactformsetting = HLConstants.ContactFormSettingItemId;
            }
            return new ContactFormSettingItem(SitecoreUtil.GetItem(Settings.GetSetting(contactformsetting)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ContactFormSettingItem GetSubscribeFormSettings(Item item)
        {
            string contactformsetting = HLConstants.TopicCenterSubscribeFormItemId;

            return new ContactFormSettingItem(SitecoreUtil.GetItem(Settings.GetSetting(contactformsetting)));
        }


        /// <summary>
        /// GetToEmail for RSVP Form on Events Page
        /// </summary>
        /// <param name="eventsItem">Event Item</param>
        /// <returns></returns>
        public static string GetRsvpToEmail(EventsItem eventsItem)
        {
            string toEmail = string.Empty;
            if (eventsItem != null)
            {
                toEmail = eventsItem.RSVPToEmail;

                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    ContactFormSettingItem rsvpSettingItem = GetContactFormSettings(eventsItem.InnerItem);
                    string fieldValue = string.Empty;
                    if (rsvpSettingItem != null && rsvpSettingItem.InnerItem != null && rsvpSettingItem.InnerItem.Fields[ContactFormSettingItem.FieldNames.DefaultToEmailFieldName] != null)
                    {
                        fieldValue = rsvpSettingItem.InnerItem.Fields[ContactFormSettingItem.FieldNames.DefaultToEmailFieldName].Value;
                    }

                    toEmail = fieldValue;
                }
            }

            return toEmail;
        }

        /// <summary>
        /// Get LookupItem Name Field Value
        /// </summary>
        /// <param name="lookupitem"></param>
        /// <returns></returns>
        public static string GetLookupItemValue(Item lookupitem)
        {
            if (lookupitem == null || !lookupitem.TemplateID.Equals(LookupItemItem.TemplateID))
            {
                return string.Empty;
            }

            return lookupitem.ItemTitle(LookupItemItem.FieldNames.NameFieldName) ?? string.Empty;
        }

        public static string GetLookupItemShortForm(Item lookupItem)
        {
            if (lookupItem == null || !lookupItem.TemplateID.Equals(LookupItemItem.TemplateID))
            {
                return string.Empty;
            }

            return ((LookupItemItem)lookupItem).ShortForm;
        }

        public static OfficeDetails GetOfficedata(Item item, bool isEditable = false)
        {
            OfficeDetails officeDetails = null;
            if (item != null)
            {
                try
                {

                    var officeLocation = item.ID.ToString();
                    EDSearch.EDSearch eDSearchObject = new EDSearch.EDSearch();
                    string searchQuery = string.Empty;

                    if (!string.IsNullOrEmpty(officeLocation))
                    {
                        searchQuery = "template|" + OfficeLocationsItem.TemplateID + ";" +
                                      SolrSearchFieldsConstant.OfficeLocation + "|" +
                                      officeLocation + ";" + SolrSearchFieldsConstant.Language + "|" + SitecoreUtil.GetContextLanguage();
                    }
                    List<Item> listItems = new List<Item>();
                    double latitude = double.MinValue;
                    double longitude = double.MinValue;
                    officeDetails = new OfficeDetails();
                    if (searchQuery != string.Empty)
                    {
                        listItems.AddRange(eDSearchObject.SearchResult(searchQuery, null));
                    }
                    if (!listItems.Any())
                    {
                        searchQuery = "template|" + OfficeLocationsItem.TemplateID + ";" +
                                          SolrSearchFieldsConstant.OfficeLocation + "|" +
                                          officeLocation + ";" + SolrSearchFieldsConstant.Language + "|" + "en";
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            listItems.AddRange(eDSearchObject.SearchResult(searchQuery, null));
                        }
                    }
                    if (listItems.Any())
                    {


                        var listItem = listItems.FirstOrDefault();
                        if (listItem != null)
                        {
                            if (isEditable && Context.PageMode.IsExperienceEditorEditing)
                            {
                                officeDetails.OfficeName = listItem.FieldValue(OfficeLocationsItem.FieldNames.OfficeNameFieldName);
                                officeDetails.AddressLine1 = listItem.FieldValue(OfficeLocationsItem.FieldNames.AddressLine1FieldName);
                                officeDetails.AddressLine2 = listItem.FieldValue(OfficeLocationsItem.FieldNames.AddressLine2FieldName);
                                officeDetails.AddressLine3 = listItem.FieldValue(OfficeLocationsItem.FieldNames.AddressLine3FieldName);
                                officeDetails.AddressLine4 = listItem.FieldValue(OfficeLocationsItem.FieldNames.AddressLine4FieldName);
                                officeDetails.PostalCode = listItem.FieldValue(OfficeLocationsItem.FieldNames.PostalCodeFieldName);
                                officeDetails.City = listItem.FieldValue(OfficeLocationsItem.FieldNames.CityFieldName);
                                officeDetails.Phone = listItem.FieldValue(OfficeLocationsItem.FieldNames.PhoneFieldName);
                                officeDetails.Fax = listItem.FieldValue(OfficeLocationsItem.FieldNames.FaxFieldName);
                            }
                            else
                            {
                                officeDetails.OfficeName = ((OfficeLocationsItem)listItem).OfficeName;
                                officeDetails.AddressLine1 = ((OfficeLocationsItem)listItem).AddressLine1;
                                officeDetails.AddressLine2 = ((OfficeLocationsItem)listItem).AddressLine2;
                                officeDetails.AddressLine3 = ((OfficeLocationsItem)listItem).AddressLine3;
                                officeDetails.AddressLine4 = ((OfficeLocationsItem)listItem).AddressLine4;
                                officeDetails.PostalCode = ((OfficeLocationsItem)listItem).PostalCode;
                                officeDetails.City = ((OfficeLocationsItem)listItem).City;
                                officeDetails.Phone = ((OfficeLocationsItem)listItem).Phone;
                                officeDetails.Fax = ((OfficeLocationsItem)listItem).Fax;
                            }
                            var countryItem = SitecoreUtil.GetItem(((OfficeLocationsItem)listItem).Country);
                            if (countryItem != null)
                            {
                                officeDetails.Country = countryItem.ItemTitle(LookupItemItem.FieldNames.NameFieldName);
                            }
                            //officeDetails.Country = SitecoreUtil.GetItem(((OfficeLocationsItem)listItem).Country).ItemTitle(LookupItemItem.FieldNames.NameFieldName);

                            string stateName = string.Empty;
                            var stateId = ((OfficeLocationsItem)listItem).StateProvince;
                            var stateItem = SitecoreUtil.GetItem(stateId);
                            if (stateItem != null)
                            {
                                officeDetails.stateItem = stateItem;
                                stateName = stateItem.ItemTitle(StateLookupItem.FieldNames.NameFieldName);
                                officeDetails.StateCode = stateItem.ItemTitle(StateLookupItem.FieldNames.StateCodeFieldName);
                            }
                            officeDetails.State = stateName;

                            if (!string.IsNullOrWhiteSpace(((OfficeLocationsItem)listItem).Latitude) && !string.IsNullOrWhiteSpace(((OfficeLocationsItem)listItem).Longitude))
                            {
                                Double.TryParse(((OfficeLocationsItem)listItem).Latitude, out latitude);
                                Double.TryParse(((OfficeLocationsItem)listItem).Longitude, out longitude);
                                officeDetails.Latitude = latitude;
                                officeDetails.Longitude = longitude;
                            }



                            if (latitude.Equals(double.MinValue) || longitude.Equals(double.MinValue))
                            {
                                officeDetails.HasMap = false;
                            }
                            else
                            {
                                officeDetails.HasMap = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager<ILogProvider>.Error(ex.Message, ex, typeof(BasicUtil));
                }

                return officeDetails;
            }

            return null;
        }


        /// <summary>
        /// Retrieves Universal Datetime from the time in some different format
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="defaultTimeZone"></param>
        /// <returns></returns>
        public static DateTime GetUniversalDateTime(DateTime stamp, TimeZoneInfo defaultTimeZone)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(stamp, defaultTimeZone);
            return utcTime;
        }


        /// <summary>
        /// Converts time from the UTC to specified format
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUtc(DateTime stamp, TimeZoneInfo timeZone)
        {
            DateTime value = TimeZoneInfo.ConvertTimeFromUtc(stamp, timeZone);
            return value;
        }


        /// <summary>
        /// Finds systemTimeZone from TimeZoneItem's Id  and internally calls GetDateTimeFromUtc method
        /// which converts time from UTC to specified format
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="convertTimeInSpecifiedTimezone"> </param>
        /// <returns></returns>
        public static DateTimeEntry GetTime(TimeZoneItem timezone, DateTime startDate, DateTime endDate, bool convertTimeInSpecifiedTimezone = true)
        {
            DateTimeEntry dateTimeEntry = null;
            if (timezone != null)
            {
                try
                {
                    string timeZoneId = timezone.TimeZoneKey;
                    string timeZoneDisplayText = timezone.TimeZoneValue;
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                    if (convertTimeInSpecifiedTimezone && timeZoneInfo != null)
                    {
                        if (!startDate.Equals(DateTime.MinValue))
                        {
                            startDate = GetDateTimeFromUtc(startDate, timeZoneInfo);
                        }
                        if (!endDate.Equals(DateTime.MinValue))
                        {
                            endDate = GetDateTimeFromUtc(endDate, timeZoneInfo);
                        }
                    }

                    dateTimeEntry = new DateTimeEntry { StartDate = startDate, EndDate = endDate, TimeZoneDisplayText = timeZoneDisplayText };
                }
                catch (Exception ex)
                {
                    LogManager<ILogProvider>.Error(ex.Message, ex, typeof(Event));
                }
            }

            return dateTimeEntry;
        }

        /// <summary>
        /// Finds SystemTimeZoneById and internally calls GetDateTimeFromUtc method
        /// which converts dateTime from Utc to specified timeZone
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeFromUtc(TimeZoneItem timezone, DateTime dateTime)
        {
            if (timezone != null)
            {
                string timeZoneId = timezone.TimeZoneKey;
                if (!string.IsNullOrWhiteSpace(timeZoneId))
                {
                    // Finds systemTimeZone from TimeZoneItem's Id
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    dateTime = GetDateTimeFromUtc(dateTime, timeZoneInfo); // Converts time from the UTC to specified format
                }
            }

            return dateTime;
        }

        /// <summary>
        /// Checks if startDate and endDate are same, and if same then is startTime and endDTime are also same
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public static ShowStartAndEndDateTime ShowStartDateEndDate(DateTime startDate, DateTime endDate)
        {
            ShowStartAndEndDateTime showStartAndEndDateTime = null;
            if (startDate == DateTime.MinValue)
            {
                showStartAndEndDateTime = new ShowStartAndEndDateTime
                {
                    ShowStartDate = false,
                    ShowEndDate = false,
                    ShowEndTime = false
                };

            }
            else if (endDate == DateTime.MinValue)
            {
                showStartAndEndDateTime = new ShowStartAndEndDateTime
                {
                    ShowStartDate = true,
                    ShowEndDate = false,
                    ShowEndTime = false
                };

            }
            else if (startDate.Date > endDate.Date)
            {
                showStartAndEndDateTime = new ShowStartAndEndDateTime
                {
                    ShowStartDate = true,
                    ShowEndDate = false,
                    ShowEndTime = false
                };

            }
            else if (startDate.Date < endDate.Date)
            {
                showStartAndEndDateTime = new ShowStartAndEndDateTime
                {
                    ShowStartDate = true,
                    ShowEndDate = true,
                    ShowEndTime = true
                };

            }
            else if (startDate.Date == endDate.Date)
            {
                if (Math.Abs(startDate.TimeOfDay.TotalSeconds - endDate.TimeOfDay.TotalSeconds) <= 0.001)
                {
                    showStartAndEndDateTime = new ShowStartAndEndDateTime
                    {
                        ShowStartDate = true,
                        ShowEndDate = false,
                        ShowEndTime = false
                    };
                }
                else
                {
                    showStartAndEndDateTime = new ShowStartAndEndDateTime
                    {
                        ShowStartDate = true,
                        ShowEndDate = false,
                        ShowEndTime = true
                    };

                }
            }

            return showStartAndEndDateTime;
        }

        /// <summary>
        /// checks if Start DateTime is greater than Current DateTime
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public static bool IsFutureDate(DateTime startDate)
        {
            if (DateTime.Compare(startDate, DateTime.Now) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<MenuItem> GetOfficeByregion(Item item)
        {
            List<MenuItem> lstMenu = new List<MenuItem>();
            lstMenu.Clear();
            var itemList = item.Axes.GetDescendants().Where(a => a.TemplateID.ToString() == OfficePageItem.TemplateID.ToString()).ToList();
            if (itemList.Any())
            {
                foreach (var itm in itemList)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.DisplayName = itm.ItemTitle(RegionPageItem.FieldNames.TitleFieldName);
                    menuItem.Url = SitecoreUtil.GetItemUrlById(itm.ID.ToString());
                    lstMenu.Add(menuItem);
                }
            }

            return lstMenu;
        }


        /// <summary>
        /// Embeds the company logo into the given mail message 
        /// </summary>
        /// <param name="imageurl"></param>
        /// <param name="messagebody"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        public static AlternateView EmbedCompanyLogo(string imageurl, string messagebody, string contentid)
        {
            byte[] imgByte = null;
            using (var webClient = new WebClient())
            {
                webClient.UseDefaultCredentials = true;
                Uri uri = new Uri(imageurl);
                imgByte = webClient.DownloadData(uri);
            }
            MemoryStream memStream = new MemoryStream(imgByte);

            AlternateView av = AlternateView.CreateAlternateViewFromString(messagebody, null, System.Net.Mime.MediaTypeNames.Text.Html);

            LinkedResource headerImage = new LinkedResource(memStream, System.Net.Mime.MediaTypeNames.Image.Jpeg);
            headerImage.ContentId = contentid;
            headerImage.ContentType = new ContentType("image/jpg");
            av.LinkedResources.Add(headerImage);

            return av;
        }

        /// <summary>
        /// Initialize ItemCountPageSettingsItem to get item count setting
        /// </summary>
        /// <returns>Object of ItemCountPageSettingsItem</returns>
        private static ItemCountPageSettingsItem GetItemCountPageSettings(Database db = null)
        {
            return new ItemCountPageSettingsItem(SitecoreUtil.GetItem(Settings.GetSetting(HLConstants.ItemCountPageSettingsItemId), db));
        }


        /// <summary>
        /// Initialize ItemCountPageSettingsItem to get item count setting
        /// </summary>
        /// <returns>Object of ItemCountPageSettingsItem</returns>
        private static MegaMenuItemsCountSettingItem GetMegaMenuItemsCountSettings(Database db = null)
        {
            return new MegaMenuItemsCountSettingItem(SitecoreUtil.GetItem("{C903CC2B-2D0C-463F-B540-5FB376A4F4E4}", db));
        }


        /// <summary>
        /// Get Item Count
        /// </summary>
        /// <returns>Item Count</returns>
        public static int GetItemCountPageSettings(string fieldName, int defaultCount = 0, Database db = null)
        {
            ItemCountPageSettingsItem pageSettingsItem = GetItemCountPageSettings(db);
            int itemsCount = 0;
            if (pageSettingsItem.InnerItem.Fields[fieldName] != null)
            {
                bool hasValue = int.TryParse(pageSettingsItem.InnerItem.Fields[fieldName].Value, out itemsCount);
                if (!hasValue || itemsCount == 0)
                {
                    itemsCount = defaultCount;
                }
            }
            return itemsCount;
        }


        /// <summary>
        /// Get Item Count
        /// </summary>
        /// <returns>Item Count</returns>
        public static int GetMegaMenuItemCountPageSettings(string fieldName, int defaultCount = 0, Database db = null)
        {
            MegaMenuItemsCountSettingItem pageSettingsItem = GetMegaMenuItemsCountSettings(db);
            int itemsCount = 0;
            if (pageSettingsItem.InnerItem.Fields[fieldName] != null)
            {
                bool hasValue = int.TryParse(pageSettingsItem.InnerItem.Fields[fieldName].Value, out itemsCount);
                if (!hasValue || itemsCount == 0)
                {
                    itemsCount = defaultCount;
                }
            }
            return itemsCount;
        }



        /// <summary>
        /// Returns field value of the field being passed
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetItemCountPageSettingsFieldValue(string fieldName)
        {
            ItemCountPageSettingsItem pageSettingsItem = GetItemCountPageSettings();
            string fieldValue = string.Empty;
            if (pageSettingsItem.InnerItem.Fields[fieldName] != null)
            {
                fieldValue = pageSettingsItem.InnerItem.Fields[fieldName].Value;
            }
            return fieldValue;
        }


        /// <summary>
        /// Get truncated short text of Item- If Abstract text is empty then Description text is used
        /// </summary>
        /// <param name="item"></param>
        /// <param name="maxlength"></param>
        /// <param name="trailingText"></param>
        /// <param name="abstractText"> </param>
        /// <returns></returns>
        public static string GetAbstractData(Item item, int maxlength, string trailingText, string abstractText = "")
        {
            string abstractContent = string.Empty;


            if (item != null)
            {
                try
                {

                    abstractContent = string.IsNullOrWhiteSpace(abstractText) ? ((NarrativesItem)item).Abstract : abstractText;

                    if (string.IsNullOrEmpty(abstractContent))
                    {

                        //Take description first paragraph (p tag, br tag) if available for short text,if not then whole decription
                        string descText = ((NarrativesItem)item).Description;
                        if (!string.IsNullOrEmpty(descText))
                        {

                            string description = descText.Trim();



                            int desLen = description.Length;
                            int desP = description.IndexOf("</p>", StringComparison.OrdinalIgnoreCase) + 4;


                            if (description.ToLower().StartsWith("<p>") && description.ToLower().Contains("</p>"))
                            {


                                abstractContent = description.Substring(0, description.IndexOf("</p>", StringComparison.OrdinalIgnoreCase) + 4);

                            }
                            else if (description.ToLower().Contains("<br") && !description.ToLower().StartsWith("<p>"))
                            {
                                string array = description.Substring(0,
                                                                     description.IndexOf("<br",
                                                                                         StringComparison.OrdinalIgnoreCase));
                                abstractContent = string.Concat("<p>", array, "</p>");

                            }

                            else
                            {
                                abstractContent = description;


                            }
                        }


                    }
                    if (!string.IsNullOrEmpty(abstractContent))
                    {
                        try
                        {
                            abstractContent = TruncateHtml(abstractContent, maxlength, trailingText);

                        }
                        catch (Exception exception)
                        {
                            Sitecore.Diagnostics.Log.Error(" Inside exception " + item.ID.ToString() + exception.Message, exception,
                                                   "GetAbstractData");
                        }

                        HtmlDocument doc = new HtmlDocument();



                        doc.LoadHtml(abstractContent);

                        var elementsWithStyleAttribute = doc.DocumentNode.SelectNodes("//@style");

                        if (elementsWithStyleAttribute != null && elementsWithStyleAttribute.Any())
                        {

                            foreach (var element in elementsWithStyleAttribute)
                            {

                                element.Attributes["style"].Remove();
                            }
                            abstractContent = doc.DocumentNode.OuterHtml;
                        }

                    }


                }
                catch (Exception exception)
                {
                    Sitecore.Diagnostics.Log.Error("TruncateHtml > HoganLovells.CommonLibrary > for Item " + item.ID.ToString(), exception,
                                           "GetAbstractData");
                }

            }

            return Regex.Replace(abstractContent, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="html"> </param>
        /// <param name="maxCharacters"> </param>
        /// <param name="trailingText"> </param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            // find the spot to truncate
            // count the text characters and ignore tags
            var textCount = 0;
            var charCount = 0;
            var ignore = false;
            foreach (char c in html)
            {
                charCount++;
                if (c == '<')
                    ignore = true;
                else if (!ignore)
                    textCount++;

                if (c == '>')
                    ignore = false;

                // stop once we hit the limit
                if (textCount >= maxCharacters)
                    break;
            }

            // Truncate the html and keep whole words only
            var trunc = new StringBuilder(html.TruncateWords(charCount));

            // keep track of open tags and close any tags left open
            var tags = new Stack<string>();
            var matches = Regex.Matches(trunc.ToString(),
                @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var closeTag = match.Groups["closeTag"].Value;

                    // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                        tags.Push(tag);

                    // pop from stack if close tag
                    else if (!string.IsNullOrEmpty(closeTag))
                    {
                        // pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Pop() != closeTag && tags.Count > 0)
                        { }
                    }
                }
            }

            if (html.Length > charCount)
                // add the trailing text
                trunc.Append(trailingText);

            // pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                trunc.Append("</");
                trunc.Append(tags.Pop());
                trunc.Append('>');
            }

            return trunc.ToString();
        }

        #region Truncate Private Functions

        /// <summary>
        /// Truncates text to a number of characters
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <returns></returns>
        private static string Truncate(this string text, int maxCharacters)
        {
            return text.Truncate(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text to a number of characters and adds trailing text, i.e. elipses, to the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        private static string Truncate(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;
            return text.Substring(0, maxCharacters) + trailingText;
        }


        /// <summary>
        /// Truncates text and discards any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <returns></returns>
        private static string TruncateWords(this string text, int maxCharacters)
        {
            return text.TruncateWords(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text and discards any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        private static string TruncateWords(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;

            string truncatedText = text.Truncate(maxCharacters);

            string wholeTruncatedText = Regex.Replace(truncatedText,
                                                      @"\s+[^\s]+$", string.Empty,
                                                      RegexOptions.IgnoreCase | RegexOptions.Compiled);

            int closingTagLastIndex = truncatedText.LastIndexOf('>');
            if (closingTagLastIndex > wholeTruncatedText.Length)
            {
                //truncate the whole text
                return truncatedText + trailingText;
            }
            // trunctate the text, then remove the partial word at the end
            return wholeTruncatedText + trailingText;
        }

        #endregion

        public static void LoadXmltoFTP(string filePath, string FileName)
        {

            string zipPath = filePath + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".zip";
            string FileNameFinal = WebConfigurationManager.AppSettings["BioProfileFTPPath"] + FileName + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".zip";
            ZipFile.CreateFromDirectory(filePath, zipPath);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Convert.ToString(WebConfigurationManager.AppSettings["BioProfileFTPPath"]) + FileName + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".zip");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(Convert.ToString(WebConfigurationManager.AppSettings["BioProfileFTPUserName"]), Convert.ToString(WebConfigurationManager.AppSettings["BioProfileFTPPassword"]));
            if (File.Exists(zipPath))
            {
                StreamReader sourceStream = new StreamReader(zipPath);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }
        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;

            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public static string RemoveStyleAttribute(string html)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(html))
                {
                    var document = new HtmlDocument();
                    document.LoadHtml(html);
                    foreach (var eachNode in document.DocumentNode.SelectNodes("//*"))
                    {
                        eachNode.Attributes.Remove("style");
                    }
                    html = document.DocumentNode.OuterHtml;
                }
            }
            catch (Exception ex)
            {
                LogManager<ILogProvider>.Error(ex.Message, ex, typeof(BasicUtil));
            }

            return html;
        }

        /// <summary>
        /// Done to avoid different language opening on two tabs of same browser
        /// </summary>
        public static void SetContextLanguage()
        {
            string currentLanguage = SitecoreUtil.GetContextLanguage();
            string contextLang = Context.Request.QueryString[QuerystringParameters.Language];
            Language lang;
            if (!string.IsNullOrEmpty(contextLang) && contextLang.ToLower() != currentLanguage.ToLower() && Language.TryParse(contextLang, out lang))
            {
                Context.SetLanguage(lang, true);
            }
        }

        public static string AddTitleAltAttribute(string html)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(html))
                {
                    var document = new HtmlAgilityPack.HtmlDocument();
                    document.LoadHtml(html);
                    foreach (var eachNode in document.DocumentNode.SelectNodes("//*"))
                    {
                        if (eachNode.Name.ToLower() == "img")
                        {
                            if (eachNode.Attributes != null && !eachNode.Attributes.Any(p => p.Name.ToLower() == "alt"))
                            {
                                eachNode.Attributes.Add("alt", eachNode.InnerText);
                            }
                        }
                        else if (eachNode.Name.ToLower() == "a")
                        {
                            if (eachNode.Attributes != null && !eachNode.Attributes.Any(p => p.Name.ToLower() == "title"))
                            {
                                eachNode.Attributes.Add("title", eachNode.InnerText);
                            }
                            else if (eachNode.Attributes != null && eachNode.Attributes.Any(p => p.Name.ToLower() == "title") && eachNode.InnerText == "#$@")
                            {
                                if (!string.IsNullOrWhiteSpace(HLConstants.HomeItemId.ToString()))
                                {
                                    Item itemPage = Sitecore.Context.Database.GetItem(HLConstants.HomeItemId);
                                    if (eachNode.Attributes["title"] != null)
                                    {
                                        eachNode.Attributes["title"].Value = itemPage.Name;
                                    }
                                }
                            }
                        }
                    }
                    html = document.DocumentNode.OuterHtml;
                }
            }
            catch (Exception ex)
            {
                LogManager<ILogProvider>.Error(ex.StackTrace, ex, typeof(BasicUtil));
            }

            return html;
        }

        public static string GetPrivacyPolicyLink()
        {
            string label = GetDictionary(Settings.GetSetting(DictionaryConstants.PleaseReadOurPrivacyPolicySetting));
            try
            {
                Item privacyFooterLinkItem = SitecoreUtil.GetItem(HLConstants.PrivacyFooterLinkItemId);
                label = string.Format(label, "<a href =\"" + SitecoreUtil.GetLinkUrl(((LinkItem)privacyFooterLinkItem).LinkURL) + "\" title =\"" + privacyFooterLinkItem.ItemTitle() + "\">", "</a>");
            }
            catch (Exception ex)
            {
                LogManager<ILogProvider>.Error(ex.Message, ex, typeof(BasicUtil));
            }

            return label;
        }

        /// <summary>
        /// Embeds the company logo into the given mail message 
        /// </summary>
        /// <param name="imageurl"></param>
        /// <param name="messagebody"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        public static AlternateView EmbedCompanyLogoForSubscription(string imageurl, string messagebody, string contentid)
        {
            byte[] imgByte;
            using (var webClient = new WebClient())
            {
                webClient.UseDefaultCredentials = true;
                Uri uri = new Uri(imageurl);
                imgByte = webClient.DownloadData(uri);
            }
            MemoryStream memStream = new MemoryStream(imgByte);

            AlternateView av = AlternateView.CreateAlternateViewFromString(messagebody, null, MediaTypeNames.Text.Html);
            LinkedResource headerImage = new LinkedResource(memStream);
            headerImage.ContentId = contentid;
            av.LinkedResources.Add(headerImage);

            return av;
        }

        public static string GetMediaFilePath(Item item)
        {
            string mediaUrl = string.Empty;
            if (item != null)
            {
                if (item.TemplateID == PublicationPageItem.TemplateID && item.Fields[PublicationPageItem.FieldNames.PdfAttachmentFieldName] != null)
                {
                    FileField fileField = item.Fields[PublicationPageItem.FieldNames.PdfAttachmentFieldName];
                    if (fileField != null && fileField.MediaItem != null)
                    {
                        mediaUrl = MediaManager.GetMediaUrl(fileField.MediaItem);
                    }
                }

            }
            return mediaUrl;
        }



        /// <summary>
        /// Remove special characters
        /// </summary>
        /// <returns></returns>
        public static string RemoveSpecialChars(string strText)
        {
            try
            {
                strText = strText.Replace("[", "");
                strText = strText.Replace("]", "");
                strText = strText.Replace("{", "");
                strText = strText.Replace("}", "");
                strText = strText.Replace("\"", "");
                strText = strText.Replace("/", "");
                strText = strText.Replace("*", "");
                strText = strText.Replace("?", "");
                strText = strText.Replace(">", "");
                strText = strText.Replace("<", "");
                strText = strText.Replace("_", "");
                strText = strText.Replace("!", "");
                strText = strText.Replace("^", "");
                strText = strText.Replace("+", "");
                strText = strText.Replace("~", "");
                strText = strText.Replace("&", "");
                strText = strText.Replace(":", "");
                strText = strText.Replace("'", "");
                strText = strText.Replace("’", "");
                strText = strText.Replace("£", "");
                strText = strText.Replace("$", "");
                strText = strText.Replace("#39;", "");
                strText = strText.Replace("`", "");
                strText = strText.Replace("%", "");
                strText = strText.Replace("(", "");
                strText = strText.Replace(")", "");
                strText = strText.Replace("=", "");
                strText = strText.Replace(".", "");
                strText = strText.Replace("“", "");
                strText = strText.Replace("”", "");
                strText = strText.Replace(",", "");
                strText = strText.Replace("–", "");
                strText = strText.Replace("€", "");
                strText = strText.Replace("©", "");
                strText = strText.Replace("@", "");
                strText = strText.Replace(";", "");
                strText = strText.Replace("—", "");
                strText = strText.Replace("•", "");
                strText = strText.Replace("´", "");
                strText = strText.Replace("  ", " ");

                strText = strText.Trim();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("RemoveSpecialChars " + ex, typeof(BasicUtil));
            }
            return strText;
        }


        public static bool CheckForAccentedCharacters(string fieldName)
        {
            bool hasAccentedCharacters = false;
            foreach (var singleChar in fieldName)
            {
                for (int i = 0x00C0; i <= 0x17F; i++)
                {
                    //back to hex

                    if (singleChar == i)
                    {
                        hasAccentedCharacters = true;
                        break;
                    }
                }
            }

            return hasAccentedCharacters;
        }



        public static string RemoveAccentedCharacters(string fieldName)
        {
            string textWithoutAccentedCharacters = fieldName;
            foreach (var singleChar in fieldName)
            {
                for (int i = 0x00C0; i <= 0x17F; i++)
                {
                    //back to hex

                    if (singleChar == i)
                    {
                        textWithoutAccentedCharacters = textWithoutAccentedCharacters.Replace(Convert.ToChar(singleChar).ToString(CultureInfo.InvariantCulture), "");
                    }
                }
            }

            return textWithoutAccentedCharacters;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        /// <summary>
        /// Replace accented characters with English letters
        /// Note: It won't work for all accented characters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }


        /// <summary>
        /// Return local part from email address i.e. before "@" part
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string ReturnEmailLocalPart(string email)
        {
            string emailLocalPart = string.Empty;

            if (!string.IsNullOrEmpty(email))
            {
                emailLocalPart = email.Split('@')[0];
            }

            return emailLocalPart;
        }

        public static string GetHtmlInnerText(string htmlText)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.InnerText;
        }

        public static string GetFirstParagraphInnerHtml(string htmlText)
        {
            string descriptionText = string.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//p");
            if (htmlNodes != null && !string.IsNullOrEmpty(htmlNodes[0].InnerHtml))
            {
                descriptionText = htmlNodes[0].InnerHtml;
            }
            return descriptionText;
        }

        public static string RemoveFirstParagraph(string htmlText)
        {
            string text = string.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//p");
            if (htmlNodes != null && !string.IsNullOrEmpty(htmlNodes[0].InnerHtml))
            {
                htmlNodes[0].Remove();
            }
            return doc.DocumentNode.OuterHtml;
        }

        public static string GetNWordsWithTrailingCharacters(string htmlText, int wordCount)
        {
            string trailingText = HLConstants.TrailingText;
            int totalWords = 0;
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> words = htmlText.Split();
            totalWords = words.Count();

            words = words.Take(wordCount);

            foreach (string word in words)
            {
                sb.Append(word + " ");
            }

            if (words.Count() < totalWords)
            {
                sb.Append(trailingText);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Seprates the data into description and abstract
        /// </summary>
        /// <param name="abstractFieldData">Text in Abstract field of the Item</param>
        /// <param name="descriptionFieldData">Text in Description field of the Item</param>
        /// <param name="abstractText">Returns the Abstract</param>
        /// <param name="descriptionText">Returns the Description</param>
        public static void GetAbstractAndDescriptionText(string abstractFieldData, string descriptionFieldData, out string abstractText, out string descriptionText)
        {
            abstractText = string.Empty;
            descriptionText = string.Empty;

            //To seprate the data into abstract and description
            if (!string.IsNullOrEmpty(abstractFieldData))
            {
                if (!string.IsNullOrWhiteSpace(BasicUtil.GetFirstParagraphInnerHtml(abstractFieldData)))
                {
                    descriptionText = BasicUtil.RemoveFirstParagraph(abstractFieldData) + descriptionFieldData;
                    abstractText = BasicUtil.GetFirstParagraphInnerHtml(abstractFieldData);
                }
            }
            else if (!string.IsNullOrEmpty(descriptionFieldData))
            {
                if (!string.IsNullOrWhiteSpace(BasicUtil.GetFirstParagraphInnerHtml(descriptionFieldData)))
                {
                    abstractText = BasicUtil.GetFirstParagraphInnerHtml(descriptionFieldData);
                    descriptionText = BasicUtil.RemoveFirstParagraph(descriptionFieldData);
                }
                else
                {
                    abstractText = descriptionFieldData;
                }

            }//End of - To seprate the data into abstract and description

        }

        public static bool IsTwitterFeedsTagged(Item currentItem)
        {
            if (currentItem != null && currentItem.IsDerived(TwitterFeedsHandlerItem.TemplateID))
            {
                Item[] twitterhandlers = ((TwitterFeedsHandlerItem)currentItem).TwitterAccountTypeItems;

                return (twitterhandlers != null && twitterhandlers.Length > 0) ? true : false;
            }
            return false;
        }


        public static string RemoveQueryStringByKey(string url, string key)
        {
            if (!string.IsNullOrWhiteSpace(url) && url.Contains(key))
            {
                var uri = new Uri(url.ToAbsoluteUrl());

                // this gets all the query string key value pairs as a collection
                var newQueryString = HttpUtility.ParseQueryString(uri.Query);

                // this removes the key if exists
                newQueryString.Remove(key);

                // this gets the page path from root without QueryString
                string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

                return newQueryString.Count > 0
                     ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                     : pagePathWithoutQueryString;
            }

            return url;
        }

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }


        public static NameValueCollection RemoveKeyValueFromQueryString(NameValueCollection queryString, string queryStringKey)
        {
            try
            {
                if (queryString != null && !string.IsNullOrWhiteSpace(queryStringKey))
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    // make collection editable
                    isreadonly.SetValue(queryString, false, null);
                    // remove
                    queryString.Remove(queryStringKey);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("BasicUtil >> RemoveKeyValueFromQueryString >> " + ex.Message + "  " + ex.StackTrace, typeof(BasicUtil));
            }

            return queryString;
        }

        /// <summary>
        /// Remove  html tags from string except nbsp;
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string RemoveHtmlButNotNBSP(string strSource)
        {
            string result = string.Empty;
            try
            {
                result = !string.IsNullOrEmpty(strSource) ?
               Regex.Replace(strSource, "<(.|\n)*?>", "").Replace("&nbsp;", " ").Replace("\n", string.Empty).Replace("\r", string.Empty).Trim() : string.Empty;
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("RemoveHtml > HoganLovells.CommonLibrary > strSource: " + strSource, exception, typeof(BasicUtil));
            }
            return result;
        }

    }

    public class OfficeDetails
    {
        public string OfficeName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string CityStateCountry { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Venue { get; set; }
        public string AdditionalInfo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Fax { get; set; }
        public bool HasMap { get; set; }
        public Item stateItem { get; set; }
    }


    public class DateTimeEntry
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeZoneDisplayText { get; set; }
    }

    public class ShowStartAndEndDateTime
    {
        public bool ShowStartDate { get; set; }
        public bool ShowEndDate { get; set; }
        public bool ShowEndTime { get; set; }
    }

    /// <summary>
    /// Bio Feeds Task
    /// This class is used by scheduler and run once in a day
    /// </summary>
    public class BioFeedsTask
    {
        /// <summary>
        /// Execute method is called by Sitecore scheduler
        /// </summary>
        /// <param name="items"></param>
        /// <param name="command"></param>
        /// <param name="schedule"></param>
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {

            string value = string.Empty;

            try
            {
                string bioFeedsUrl = Settings.GetSetting(HLConstants.BioFeedsURL);
                value = "URL =" + bioFeedsUrl;

                if (!string.IsNullOrEmpty(bioFeedsUrl))
                {
                    //May need to execute for two different websites like PROD1, PROD2
                    foreach (var feedsUrl in bioFeedsUrl.Split(new char[] { ',' }))
                    {
                        try
                        {
                            // Create web client.
                            WebClient webClient = new WebClient();
                            // Download string.
                            value = webClient.DownloadString(feedsUrl);
                        }
                        catch (Exception ex)
                        {
                            value += " Inner " + value + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                value = value + ex.Message;
            }


        }
    }

    /// <summary>
    /// Scheduler for inserting master details in BioPortal
    /// </summary>
    public class BioPortalDataGateway
    {
        /// <summary>
        /// Scheduler executes method for inserting master details in BioPortal
        /// </summary>
        /// <param name="items"></param>
        /// <param name="command"></param>
        /// <param name="schedule"></param>
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {

            string value = string.Empty;

            try
            {
                value += "Executing Service Start " + Environment.NewLine;
                //Service
                InsertItemIntoSql("{290FFF33-7DC8-42BA-82FE-CF5DA22F7BEE}", "{964A5767-57C0-4E8C-9F3B-BFD2DF77866C}", "Proc_tbl_MasterServiceSave", "ServiceGuid"
                    , "Proc_DeleteUpdatetbl_MasterServiceByStatus");
                value += "Executing Service Completed" + Environment.NewLine;

                value += "Executing Industry Start " + Environment.NewLine;
                //Industry
                InsertItemIntoSql("{264298A0-254F-408A-9C2E-75829F4504BF}", "{BC22B432-3384-4727-90C7-6E76753A1C80}", "Proc_tbl_MasterIndustrySave", "IndustryGuid",
                    "Proc_DeleteUpdatetbl_MasterIndustryByStatus");
                value += "Executing Industry Completed" + Environment.NewLine;

                value += "Executing Position Start " + Environment.NewLine;
                //Position
                InsertItemIntoSql("{6FAB8C27-6AF6-4F86-9997-F5A6F2C5FDAF}", "{2200DB4F-50DE-4524-979A-110699D78FCB}", "Proc_tbl_MasterPositionSave", "PositionGuid"
                    , "Proc_DeleteUpdatetbl_MasterPositionByStatus");
                value += "Executing Position Completed" + Environment.NewLine;

                value += "Executing Location Start " + Environment.NewLine;
                //Location
                InsertItemIntoSql("{20FBB520-86C1-4090-832D-8C7B0DB4EBCC}", "{09E2E1B8-104C-4E36-8168-73909524FE12}", "Proc_tbl_MasterLocationSave", "LocationGuid"
                    , "Proc_DeleteUpdatetbl_MasterLocationByStatus");
                value += "Executing Location Completed" + Environment.NewLine;

                value += "Executing Area Of Focus Start " + Environment.NewLine;
                //Area Of Focus
                InsertItemIntoSql("{827F16DD-152D-467D-B419-6C90F2CD576F}", "{67321BC2-F1E4-4DED-BCB4-6D786EE94902}", "Proc_tbl_MasterAreaOfFocusSave", "AreaOfFocusGuid"
                    , "Proc_DeleteUpdatetbl_MasterAreaOfFocusByStatus");
                value += "Executing Area Of Focus Completed" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                value = value + ex.Message;
            }


        }

        /// <summary>
        /// Insert items details in database
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="targetTemplateId"></param>
        /// <param name="spName"></param>
        /// <param name="guidColumnName"></param>
        /// <param name="deleteSpName"> </param>
        private void InsertItemIntoSql(string parentId, string targetTemplateId, string spName, string guidColumnName, string deleteSpName)
        {
            try
            {
                Item parentFolderItem = Database.GetDatabase("master").GetItem(new ID(parentId));
                var dataList = parentFolderItem.Axes.GetDescendants().Where(x => x.TemplateID.ToString().ToLower().Equals(targetTemplateId.ToLower().Trim())
                    && !string.IsNullOrEmpty(x.Fields["Title"].Value) && !x.Fields["Title"].Value.Equals("$name"))
                    .Select(x => new { ItemGuid = x.ID.ToString(), Name = x.Fields["Title"].Value }).OrderBy(x => x.Name).ToList();

                SqlConnection sqlcon = GetSQLConnection();
                OpenConnection(sqlcon);

                foreach (var currentItem in dataList)
                {
                    try
                    {
                        List<SqlParameter> paramList = new List<SqlParameter>
                                                           {
                                                               new SqlParameter { ParameterName = "@" + guidColumnName, Value = currentItem.ItemGuid },
                                                               new SqlParameter {ParameterName = "@LanguageCode", Value = "en"},
                                                               new SqlParameter {ParameterName = "@Name", Value = currentItem.Name}
                                                           };

                        ExecuteProc(spName, paramList, sqlcon);
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("InsertItemIntoSQL exception for " + guidColumnName + " for item name " + currentItem.Name + "->" + ex.Message, this);
                    }
                }

                ExecuteProc(deleteSpName, null, sqlcon);

                CloseConnection(sqlcon);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("InsertItemIntoSQL execution completed at " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " with error " + ex, this);
            }
        }

        /// <summary>
        /// Executes Stored procedure
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramList"></param>
        /// <param name="sqlcon"></param>
        private int ExecuteProc(string spName, List<SqlParameter> paramList, SqlConnection sqlcon)
        {
            //SqlConnection sqlcon = GetSQLConnection();
            SqlCommand cmd = new SqlCommand(spName, sqlcon);
            cmd.CommandType = CommandType.StoredProcedure;

            if (paramList != null)
            {
                cmd.Parameters.AddRange(paramList.ToArray());
            }

            //OpenConnection(sqlcon);

            return cmd.ExecuteNonQuery();

            //CloseConnection(sqlcon);
        }

        /// <summary>
        /// Close database connection
        /// </summary>
        /// <param name="sqlcon"></param>
        private void CloseConnection(SqlConnection sqlcon)
        {
            if (sqlcon.State != ConnectionState.Closed)
            {
                sqlcon.Close();
            }
        }

        /// <summary>
        /// Open Database connection
        /// </summary>
        /// <param name="sqlcon"></param>
        private void OpenConnection(SqlConnection sqlcon)
        {
            if (sqlcon.State != ConnectionState.Open)
            {
                sqlcon.Open();
            }
        }

        /// <summary>
        /// Get SQL connection for Bio Portal
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetSQLConnection()
        {
            string connectionString = Settings.GetSetting("bioportalconnectionstring");
            SqlConnection sqlcon = new SqlConnection(connectionString);
            return sqlcon;
        }
    }

    public class CustomDateProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is DateTime)) throw new NotSupportedException();

            var dt = (DateTime)arg;

            string suffix;

            if (new[] { 11, 12, 13 }.Contains(dt.Day))
            {
                suffix = "th";
            }
            else if (dt.Day % 10 == 1)
            {
                suffix = "st";
            }
            else if (dt.Day % 10 == 2)
            {
                suffix = "nd";
            }
            else if (dt.Day % 10 == 3)
            {
                suffix = "rd";
            }
            else
            {
                suffix = "th";
            }

            return string.Format("{0:MMMM} {1}{2}, {0:yyyy}", arg, dt.Day, suffix);
        }
    }

}