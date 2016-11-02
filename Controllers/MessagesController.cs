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
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));               
                City cm = new City();

                if (activity.Text.All(char.IsDigit))
                {
                    string cityName = cm.getCityNamebyPlateNumber(Convert.ToInt32(activity.Text));

                    //Activity reply = activity.CreateReply($"You sent {activity.Text} plate number which is {cityName} city in Turkey");
                    Activity reply = activity.CreateReply($"Sorguladığınız {activity.Text} plaka kodu {cityName} iline aittir.");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                }
                else
                {
                    int pNumber = cm.getPlateNumberbyCity(activity.Text);

                    //Activity reply = activity.CreateReply($"You sent {activity.Text} city name which is {pNumber} as its plate number in Turkey");
                    Activity reply = activity.CreateReply($"Sorguladığınız {activity.Text} ilinin plaka kodu: {pNumber}");
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