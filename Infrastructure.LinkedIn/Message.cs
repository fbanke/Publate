﻿namespace Infrastructure.LinkedIn
{
    public class Message
    {
        public Message(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}