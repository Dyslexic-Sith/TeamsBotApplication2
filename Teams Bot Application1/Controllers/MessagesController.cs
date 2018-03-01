using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

namespace Teams_Bot_Application1
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
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else if (activity.Type == ActivityTypes.Invoke) // Received an invoke
            {
                // Handle ComposeExtension query
                if (activity.IsComposeExtensionQuery())
                {
                    var invokeResponse = this.GetComposeExtensionResponse(activity);
                    return Request.CreateResponse<ComposeExtensionResponse>(HttpStatusCode.OK, invokeResponse);
                }
                else
                {
                    // Handle other types of invoke
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }

        private ComposeExtensionResponse GetComposeExtensionResponse(Activity activity)
        {
            var query = activity.GetComposeExtensionQueryData();

            if (query.CommandId == null || query.Parameters == null)
            {
                return null;
            }

            switch (query.CommandId)
            {
                //case "someCommand":
                //    var response = HandleSomeCommand(activity);
                //    return response;
                case "//cats":
                    
                    var response = HandleSomeCommand(activity);
                    return response;
                default:
                    throw new ArgumentException("Invalid command");
            }
        }

        private ComposeExtensionResponse HandleSomeCommand(Activity activity)
        {
            ComposeExtensionResult catResult = new ComposeExtensionResult(null, "message", null, null, "Here is a cat ┬┴┬┴┤･ω･)ﾉ├┬┴┬┴");
            ComposeExtensionResponse flyingCat = new ComposeExtensionResponse(catResult);
            return flyingCat;
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
            else if(message.Type == ActivityTypes.Message)
            {

            }

            return null;
        }
    }
}