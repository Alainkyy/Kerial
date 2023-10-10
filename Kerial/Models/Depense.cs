namespace Kerial.Models
{
    /* Une dépense est caractérisée par :

    Un utilisateur (personne qui a effectué l'achat)
    Une date
    Une nature (valeurs possibles : Restaurant, Hotel et Misc)
    Un montant et une devise
    Un commentaire
    
     */
    public class Depense
    {
        public int idDepense { get; set; }
        public int idUtilisateur { get; set; }
        public DateTime date { get; set; }
        public string nature { get; set; }
        public float montant { get; set; }
        public string devise { get; set; }
        public string commentaire { get; set; }
    }
}
