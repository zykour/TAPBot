using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class TAPGroupChat : GroupChat 
    {
        protected bool triviaRunning;
        protected int triviaID;

        public TAPGroupChat(string chatName, SteamID chatId) : base(chatName, chatId)
        {
            triviaID = -1;
            triviaRunning = false;
        }

        public void StartTrivia()
        {
            triviaRunning = true;
        }

        public void StartTrivia(int id)
        {
            triviaRunning = true;
            triviaID = id;
        }

        public void SetTriviaID(int id)
        {
            this.triviaID = id;
        }

        public void EndTrivia()
        {
            triviaRunning = false;
        }

        public bool IsTriviaRunning()
        {
            return triviaRunning;
        }
    }
}
