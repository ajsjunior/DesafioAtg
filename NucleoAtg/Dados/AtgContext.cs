using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NucleoAtg.Dados
{
    public class AtgContext : DbContext
    {
        public AtgContext(DbContextOptions<AtgContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Mensagem> Mensagens { get; set; }
    }
}
