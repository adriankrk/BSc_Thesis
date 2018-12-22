using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Katalog.WebScraping
{
    class RepustateClient {

        public static string DOMAIN = "http://api.repustate.com/v3/";
        public static string API_KEY = "5d59252afec37858d9f9b800e40fa3f5a3c04f3f";
        public static string BASE_URL = DOMAIN + API_KEY;
	    
        public static string Sentiment(string text, string lang = "en") {
            /*
             * Basic sentiment on block of text.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("lang", lang);
            return sendRequest("/score.json", "POST", postArgs);
        }
        
        public static string BulkSentiment(List<string> text_blocks, string lang = "en") {
            /*
             * text_blocks is a list of text blocks that you want to analyze in
             * bulk. IDs are automatically assigned to each text block in the
             * order they appear in the list.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("lang", lang);
            for (int i = 0; i < text_blocks.Count; i++) {
                postArgs.Add("text"+i, text_blocks[i]);
            }
            return sendRequest("/bulk-score.json", "POST", postArgs);
        }
        
        public static string chunk(string text, string lang = "en") {
            /*
             * Chunk the text and return the sentiment for each chunk.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("lang", lang);
            return sendRequest("/chunk.json", "POST", postArgs);
        }
        
        public static string topic(string text, string topics, string lang = "en") {
            /*
             * Restrict sentiment to the topic(s) specified. Comma separate
             * multiple topics.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("topics", topics);
            postArgs.Add("lang", lang);
            return sendRequest("/topic.json", "POST", postArgs);
        }
        
        public static string posTag(string text, string lang = "en") {
            /*
             * Return the part of speech tags for this text. Tags are
             * normalized to Repustate's tagset. Please see
             * https://www.repustate.com/docs for details.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("lang", lang);
            return sendRequest("/pos.json", "POST", postArgs);
        }
        
        public static string entities(string text, string lang = "en") {
            /*
             * Perform subject matter classification and named entity extraction on the text.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("lang", lang);
            return sendRequest("/entities.json", "POST", postArgs);
        }
        
        public static string cleanHtml(string url) {
            /*
             * Extract just the main content from an HTML page.
             */
		    Dictionary<string, string> getArgs = new Dictionary<string, string>();
            getArgs.Add("url", url);
            return sendRequest("/clean-html.json", "GET", getArgs);
        }
        
        public static string detectLanguage(string text) {
            /*
             * Return the two letter ISO code representing the language of the
             * text. Only certain languages are supported. See
             * https://www.repustate.com/docs for more details.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            return sendRequest("/detect-language.json", "POST", postArgs);
        }
        
        public static string addSentimentRule(string text, string sentiment, string lang = "en") {
            /* 
             * Create a custom sentiment rule. `text` can be a word or phrase
             * that you'd like to public static stringine the sentiment for.
             * `sentiment` should be one of: 
             * 1. positive
             * 2. negative
             * 3. neutral
             * 
             * Remember to commit your rules!
            */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("sentiment", text);
            postArgs.Add("lang", lang);
            return sendRequest("/sentiment-rules.json", "POST", postArgs);
        }

        public static string deleteSentimentRule(string rule_id) {
            /*
             * Delete a previously created sentiment rule.
             */
		    Dictionary<string, string> deleteArgs = new Dictionary<string, string>();
            deleteArgs.Add("rule_id", rule_id);
            return sendRequest("/sentiment-rules.json", "DELETE", deleteArgs);
        }

        public static string listSentimentRules() {
            /*
             * List the sentiment rules you've created.
             */
            return sendRequest("/sentiment-rules.json", "GET", null);
        }

        public static string commitSentimentRules() {
            /*
             * Commit any sentiment rules you've added and add them to your
             * language model.
             */
            return sendRequest("/sentiment-rules.json", "PUT", null);
        }
        
        public static string categorize(string text, string niche, string lang = "en") {
            /*
             * Categorize chunks within the text block according to the
             * pre-defined semantic categories Repustate provides for a given
             * niche (e.g. price & location within the hotel niche).
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("text", text);
            postArgs.Add("niche", niche);
            postArgs.Add("lang", lang);
            return sendRequest("/categorize.json", "POST", postArgs);
        }

        public static string addCategoryRule(string niche, string category, string query, string weight = "0", string lang = "en") {
            /*
             * Add a rule (and category, niche) by which you want to classify
             * text. If the niche and/or category previously didn't exist, they
             * will be automatically created.  The rule_id of the new rule will
             * be returned.
             */
		    Dictionary<string, string> postArgs = new Dictionary<string, string>();
            postArgs.Add("niche", niche);
            postArgs.Add("category", category);
            postArgs.Add("query", query);
            postArgs.Add("weight", weight);
            postArgs.Add("lang", lang);
            return sendRequest("/category-rules.json", "POST", postArgs);
        }

        public static string deleteCategoryRule(string rule_id) {
            /* Delete a previously created custom rule. The `rule_id` was
             * returned when add_category_rule was first called. Alternatively,
             * you can call `list_category_rules` and retrieve the rule_id that
             * way.
            */
		    Dictionary<string, string> deleteArgs = new Dictionary<string, string>();
            deleteArgs.Add("rule_id", rule_id);
            return sendRequest("/category-rules.json", "DELETE", deleteArgs);
        }

        public static string listCategoryRules() {
            /* 
             * List all custom category rules you've created.
             */
            return sendRequest("/category-rules.json", "GET", null);
        }

        /********************************UTILITY METHODS****************************************/
        public static string sendRequest(string path, string method, Dictionary<string,string> request_data) {
            
            string outputData = "";
            string data = getContentFromDictionary(request_data);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + path);
            myHttpWebRequest.Method = method;
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

            //Write the data to the stream
            if (data != null && !data.Equals("")) 
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                Stream dataStream = myHttpWebRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream streamResponse = myHttpWebResponse.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);

            Char[] readBuff = new Char[256];
            int count = streamRead.Read(readBuff, 0, 256);
            while (count > 0)
            {
                outputData += new string(readBuff, 0, count);
                count = streamRead.Read(readBuff, 0, 256);
            }

            streamResponse.Close();
            streamRead.Close();
            myHttpWebResponse.Close();

            return outputData;
        }

        private static string getContentFromDictionary(Dictionary<string,string> data) {
            string content = "";
            if (data != null) {
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in data) {
                    content += kvp.Key + "=" + System.Net.WebUtility.UrlEncode(kvp.Value);
                    content += "&";
                    i++;
                }
            }
            return content; 	        
        }
    }
}
