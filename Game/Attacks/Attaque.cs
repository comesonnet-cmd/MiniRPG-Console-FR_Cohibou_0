using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniRPG_Console_FR_Cohibou_0;
using Game.Elements;
using Game.Effects;


namespace Game.Attacks
{
    public class Attaque
    {
        public string Nom {  get; set; }
        internal Element.TYPE Type { get; set; }    
        public int Puissance { get; set; }

        public Effects.EffectsManager.EFFECT Effet { get; set; }
        // Tire à pile ou face
        public bool TirePileFace { get; set; }

        // On crée un delegate pour récupérer l'attaque bonus dynamiquement
        public Func<Attaque?>? AttaqueBonusFunc { get; set; }

        // La propriété AttaqueBonus retourne l'attaque bonus réelle si le delegate existe bien, sinon null:

        public Attaque? AttaqueBonus => AttaqueBonusFunc?.Invoke(); // Invoke() appelle le delegate stocké dans AttaqueBonusFunc


        public Attaque AttaqueImprevue { get; set; }
    }
}
