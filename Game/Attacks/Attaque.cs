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
        public string Nom;
        internal Element.TYPE Type;
        public int Puissance;

        public EffectsManager.EFFECT Effet = EffectsManager.EFFECT.Aucun;   // Par défaut : aucun effet
        // Tire à pile ou face
        public bool TirePileFace = false;           // true si l'effet doit tirer à pile ou face 
        public Attaque AttaqueBonus = null;         // attaque à lancer si Face
        public Attaque AttaqueImprevue;
    }
}
