using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace kevnls
{
    //This is the script that will handle retrieving all 
    //the story data from the story.xml file
    public class Story : MonoBehaviour
    {

        public TextAsset storyXML;
        
        private static string currentChapter = "Chapter 1";
        private static StoryContainer storyContainer;
        private static int paragraphCounter = 0;

        void Start()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(storyXML.text);
            XmlNodeReader reader = new XmlNodeReader(xmlDoc);
            storyContainer = StoryContainer.Load(reader);
        }

        public void SwitchChapter(string chapterName)
        {
            currentChapter = chapterName;
            paragraphCounter = 0;
        }


        //gets a random phrase based on the chapter and character type
        //new chapters and character types can be added to the XML file with no changes needed here
        public static string GetPhrase(string characterType)
        {
            string returnString = "";

            foreach (Chapter chapter in storyContainer.chapters)
            {
                if (chapter.title == currentChapter)
                {
                    foreach (Dialog dialog in chapter.dialogs)
                    {
                        if (dialog.characterType == characterType)
                        {
                            int intRandom = Random.Range(0, dialog.phrases.Length);
                            returnString = dialog.phrases[intRandom].phrase;
                        }
                    }
                }
            }
            return returnString;
        }

        public static string GetParagraph()
        {
            string returnString = "";

            foreach (Chapter chapter in storyContainer.chapters)
            {
                if (chapter.title == currentChapter)
                {
                    if (paragraphCounter <= chapter.paragraphs.Length)
                    {
                        returnString = chapter.paragraphs[paragraphCounter].paragraph;
                        paragraphCounter++;
                    }
                    else
                    {
                        //if we've reached the end of the chapter's paragraphs just keep returning the last one
                        returnString = chapter.paragraphs[chapter.paragraphs.Length].paragraph;
                    }
                }
            }

            return returnString;
        }
    }

    [XmlRoot("story")]
    public class StoryContainer
    {
        [XmlAttribute("title")]
        public string title;

        [XmlArray("chapters")]
        [XmlArrayItem("chapter")]
        public Chapter[] chapters;

        public static StoryContainer Load(XmlNodeReader reader)
        {
            var serializer = new XmlSerializer(typeof(StoryContainer));
            return serializer.Deserialize(reader) as StoryContainer;
        }
    }

    public class Chapter
    {
        [XmlAttribute("title")]
        public string title;

        [XmlArray("paragraphs")]
        [XmlArrayItem("paragraph")]
        public Paragraph[] paragraphs;

        [XmlArray("dialogs")]
        [XmlArrayItem("dialog")]
        public Dialog[] dialogs;
    }

    public class Paragraph
    {
        [XmlText]
        public string paragraph;
    }

    public class Dialog
    {
        [XmlAttribute("characterType")]
        public string characterType;

        [XmlArray("phrases")]
        [XmlArrayItem("phrase")]
        public Phrase[] phrases;
    }

    public class Phrase
    {
        [XmlText]
        public string phrase;
    }
}
