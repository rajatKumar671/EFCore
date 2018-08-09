using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace SomeUI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            // InsertSamurai();
            // SimpleSamuraiQuery();
            // InsertMultipleDeifferentObject();
            // MoreQueries();
            // RetrieveAndUpdateSamurai();
            // RetrieveAndUpdateMultipleSamurai();
            // MultipleDataBaseOperation();
            // InsertBattle();
            // QueryAndUpdateBattle_disconnectted();
            // AddSomeMoreSamurais();
            // DeleteWhileTracked();
            // DeleteMany();
            // InsertNewPkFkGraph();
            // InsertNewPkFkGraphMultipleChildren();
            // AddChildToExistingObjectWhileTracked(1);
            // EagerLoadSamuraiWithQuotes();
            // ProjectSomeProperies();
            // ProjectSamuraisWithQuotes();
            // FilteringWithRelatedData();
            // ModifyingRelatedDataWhenTracked();
            ModifyingRelatedDataWhenNotTracked();
        }
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            //samurai.Quotes[0].Text += "Did you hear that?";
            _context.Quotes.Remove(samurai.Quotes[1]);
            _context.SaveChanges();
        }
        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote= samurai.Quotes[0];
            quote.Text += "Did you hear that?";
            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Update(quote);
                _context.SaveChanges();
            }
        }
        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais.Where(s =>
                  s.Quotes.Any(q => q.Text.Contains("happy"))).ToList();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropertiesWithQuotes = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes.Count })
            //                               .ToList();

            // var somePropertiesWithQuotes = _context.Samurais
            //    .Select(s => new{s.Id, s.Name,
            //       HappyQuotes = s.Quotes.Where(
            //               q => q.Text.Contains("happy"))})
            //             .ToList();

            var samuraisWithHappyQuotes = _context.Samurais.Select(s => new
            {
                Samurai = s,
                Quotes = s.Quotes.Where(q => q.Text.Contains("happy")).ToList()
            }).ToList();
        }
        private static void ProjectSomeProperies()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name,s.Quotes }).ToList();
        }
        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQutes = _context.Samurais.Where(s => s.Name.Contains("sahil"))
                                                   .Include(s => s.Quotes)
                                                   .Include(s => s.SecretIdentity)
                                                   .FirstOrDefault();
        }
        private static void AddChildToExistingObjectWhileTracked(int samuraiId)
        {
            var quote=new Quote
            {
                Text = "I bet you're happy that I'have saved you!",
                SamuraiId=samuraiId
            };
            using (var newContext = new SamuraiContext())
            {
                _context.Quotes.Add(quote);
                _context.SaveChanges();
            }
        }
        private static void InsertNewPkFkGraphMultipleChildren()
        {
            var samurai = new Samurai
            {
                Name = "Kuldeep",
                Quotes = new List<Quote>
                {
                    new Quote{Text="Watch out for my sharp sword"},
                    new Quote{Text="I told you to watch out for the sharp sword! oh well"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
         private static void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Prince", Quotes = new List<Quote>
                {
                    new Quote{Text="I've come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddSomeMoreSamurais()
        {
            _context.AddRange(
                new Samurai { Name="Sahil"},
                new Samurai { Name="Prince"}
                );
            _context.SaveChanges();
        }
        private static void DeleteWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Sahil");
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void DeleteMany()
        {
            var samurais = _context.Samurais.Where(s => s.Name.Contains("u"));
            _context.Samurais.RemoveRange(samurais);
            _context.SaveChanges();
        }
        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle { Name = "Battle of Gaziabad",
              StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) });
            _context.SaveChanges();
        }
        private static void QueryAndUpdateBattle_disconnectted()
            {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var newContextInstance=new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertMultipleDeifferentObject()
        {
            var samurai = new Samurai { Name = "Rajat kumar" };
            var battle = new Battle
            {
                Name = "Battle of Nagashino",
                StartDate=new DateTime(1575,06,16),
                EndDate=new DateTime(1575,06,28)
            };
            using (var context = new SamuraiContext())
            {
                context.AddRange(samurai,battle);
                context.SaveChanges();
            }
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Rajat" };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }

        }
        private static void SimpleSamuraiQuery()
        {
            using (var context = new SamuraiContext())
            {
                var samurais = context.Samurais.ToList();
            }
        }
        private static void MoreQueries()
        {
            var name = "Rajat";
            //var samurais = _context.Samurais.Find(2);
            var lastrajat = _context.Samurais.LastOrDefault(s=> s.Name==name);
        }
        private static void RetrieveAndUpdateSamurai()
        {
            var samurais = _context.Samurais.FirstOrDefault();
            samurais.Name += "Hiro";
            _context.SaveChanges();
        }
        private static void RetrieveAndUpdateMultipleSamurai()
        {
            var samurais = _context.Samurais.ToList();
            samurais.ForEach(s => s.Name += "Hiro");
            _context.SaveChanges();
        }
        private static void MultipleDataBaseOperation()
        {
            var samurais = _context.Samurais.FirstOrDefault();
            samurais.Name += "Hiro";
            _context.Samurais.Add(new Samurai { Name = "Kiku" });
            _context.SaveChanges();
        }
    }
}
