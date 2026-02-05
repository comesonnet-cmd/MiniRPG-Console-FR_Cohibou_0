using MiniRPG_Console_FR_Cohibou_0.Characters;

namespace Game
{
    public class Attaque
    {
        public string Nom;
        public Element.TYPE ;
        public int Puissance;

        public EffetType Effet = EffetType.Aucun;   // Par défaut : aucun effet
        // Tire à pile ou face
        public bool TirePileFace = false;           // true si l'effet doit tirer à pile ou face 
        public Attaque AttaqueBonus = null;         // attaque à lancer si Face
        public Attaque AttaqueImprevue;
    }
}
