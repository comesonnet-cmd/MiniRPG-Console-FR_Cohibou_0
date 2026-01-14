namespace MiniRPG_Console_FR_Cohibou_0.Characters
{
    public enum TypeElement      // On définit les 4 types élémentaires : Automne, Penombre, Veneneux, Givre.
                                 // Ça permet de gérer facilement le type d’une attaque ou d’un personnage.
    {
        Automne,
        Penombre,
        Veneneux,
        Givre
    }

    public enum EffetType
    {
        Aucun,
        Empoisonnement,
        ConversionPenombre,
        Engluage,
        PochetteSurprise,
        ImmuGivre,
        ProtectVeneneux,
        raté,
        Torpeur
    }
}

