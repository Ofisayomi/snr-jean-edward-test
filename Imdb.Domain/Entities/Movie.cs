using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imdb.Domain.Entities;

public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id {get;set;}
        public string Keyword {get;set;}
        public string SearchResult {get;set;}
        public string ImdbId {get;set;}
        public DateTime DateCreated {get;set;} = DateTime.Now;
    }
