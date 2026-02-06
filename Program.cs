using Game;
using MiniRPG_Console_FR_Cohibou_0;

namespace MiniRPG_Console_FR_Cohibou_0
{
    class Program
    {
        static void Main()
        {
            while (true)  //While(true) ne s'arrete jamais, tu peux faire un truc comme en dessous
            {
                GameManager.LancerJeu();
            }

            // bool finish = false;
            // while(!finish){
                    //Si on appuit sur echap, finish = true, alors on stop le jeu. 
            // }
        }
    }
}
