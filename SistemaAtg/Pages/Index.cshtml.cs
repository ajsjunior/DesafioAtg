using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NucleoAtg.Dados;

namespace SistemaAtg.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AtgContext _bd;

        private const string URL_API = "http://localhost:61228/api/mensagem";

        public IndexModel(AtgContext bd)
        {
            _bd = bd;
        }

        #region Envio de Mensagem
        [BindProperty]
        public string RespostaEnvio { get; set; }

        [BindProperty]
        public Mensagem EnviarMensagem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Historico = await _bd.Mensagens.AsNoTracking().ToListAsync();
                return Page();
            }

            var enviado = false;

            // Grava mensagem no banco
            _bd.Mensagens.Add(EnviarMensagem);
            await _bd.SaveChangesAsync();
            var novoId = EnviarMensagem.Id;

            // Envia mensagem para o serviço Rest
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var stringTask = client.PutAsJsonAsync(URL_API, EnviarMensagem);

                var msg = await stringTask;
                enviado = msg.IsSuccessStatusCode;
                if (!enviado)
                    RespostaEnvio = $"Erro: {msg.ReasonPhrase}";
            }

            if (enviado)
            {
                // Recarrega a página
                return RedirectToPage("/Index");
            }
            else
            {
                // Deleta do banco a mensagem se não foi enviada para o RabbitMQ
                var registro = await _bd.Mensagens.FindAsync(novoId);
                _bd.Mensagens.Remove(registro);
                await _bd.SaveChangesAsync();

                // Permanece na página
                Historico = await _bd.Mensagens.AsNoTracking().ToListAsync();
                return Page();
            }
        }
        #endregion

        #region Histórico de Mensagens
        public IList<Mensagem> Historico { get; private set; }

        public async Task OnGetAsync()
        {
            Historico = await _bd.Mensagens.AsNoTracking().ToListAsync();
        }
        #endregion
    }
}
