using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Elements
{
    public static class Element
{
    public enum TYPE     // On définit les 4 types élémentaires : Automne, Penombre, Veneneux, Givre.
                                 // Ça permet de gérer facilement le type d’une attaque ou d’un personnage.
    {
        Automne,
        Penombre,
        Veneneux,
        Givre,
        Néant
    }

    public static double GetMultiplicateur(TYPE attaquant, TYPE defenseur)        // Crée un tableau 2D qui contient les multiplicateurs de dégâts selon le type qui attaque et celui qui défend.
    {                                                                                           // Exemple : si une attaque Automne touche un ennemi Pénombre → multiplicateur = 2 (super efficace).
                                                                                                // La fonction retourne automatiquement le multiplicateur correspondant pour n’importe quelle combinaison d’attaquant/défenseur.

        double[,] multiplicateurs = new double[4, 4];   // crée un tableau 4x4

        multiplicateurs[0, 0] = 1;                      // Automne vs Automne = normal
        multiplicateurs[0, 1] = 2;                      // Automne vs Pénombre = super efficace
        multiplicateurs[0, 2] = 0;                      // Automne vs Vénéneux = aucun dégât
        multiplicateurs[0, 3] = 1;                      // Automne vs Givré = normal

        multiplicateurs[1, 0] = 0;                      // Pénombre vs Automne = aucun dégât
        multiplicateurs[1, 1] = 2;                      // Pénombre vs Pénombre = super efficace
        multiplicateurs[1, 2] = 2;                      // Pénombre vs Vénéneux = super efficace
        multiplicateurs[1, 3] = 0;                      // Pénombre vs Givré = aucun dégât

        multiplicateurs[2, 0] = 2;                      // Vénéneux vs Automne = super efficace
        multiplicateurs[2, 1] = 1;                      // Vénéneux vs Pénombre = normal
        multiplicateurs[2, 2] = 0.5;                    // Vénéneux vs Vénéneux = résitance de 50%
        multiplicateurs[2, 3] = 0.5;                    // Vénéneux vs Givré = résitance de 50%

        multiplicateurs[3, 0] = 2;                      // Givré vs Automne = super efficace
        multiplicateurs[3, 1] = 1;                      // Givré vs Pénombre = normal
        multiplicateurs[3, 2] = 2;                      // Givré vs Vénéneux = super efficace
        multiplicateurs[3, 3] = 1;                      // Givré vs Givré = normal

        return multiplicateurs[(int)attaquant, (int)defenseur];
    }

}
}
