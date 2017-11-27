using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace TRPlateBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //Bu kod parçası sadece komut-cevap olarak çalışır
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                City cm = new City();

                //Girilen içerik sadece sayı ise Plaka kodu olduğunu anlayarak, şehir adını arıyor.
                if (activity.Text.All(char.IsDigit))
                {
                    string cityName = cm.getCityNamebyPlateNumber(Convert.ToInt32(activity.Text));

                    string message = $"Sorguladığınız {activity.Text} plaka kodu {cityName} iline aittir.";
                    if (cityName == null) message = $"Sorguladığınız {activity.Text} sorgu kayıtlarımızda bulunamadı";
                    Activity reply = activity.CreateReply(message);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                else
                {
                    //Girilen içerik sadece yazı ise Şehir adı olduğunu varsayarak, plaka kodunu arıyor.
                    int pNumber = cm.getPlateNumberbyCity(activity.Text);

                    string message = $"Sorguladığınız {activity.Text}  ilinin plaka kodu: {pNumber}";
                    if (pNumber == 0) message = $"Sorguladığınız {activity.Text} sorgu kayıtlarımızda  bulunamadı";
                    Activity reply = activity.CreateReply(message);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}