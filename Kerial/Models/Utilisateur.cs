namespace Kerial.Models
{
    /*Un utilisateur est caractérisé par :

    Un nom de famille
    Un prénom
    Une devise dans laquelle il effectue ses achats
    
     */
    public class Utilisateur
    {
        public int idUtilisateur { get; set; }
        public string nomDeFamille { get; set; }
        public string prenom { get; set; }
        public string devise { get; set; }

    }
}
