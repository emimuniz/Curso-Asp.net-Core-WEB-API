using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.Extensions.Configuration;
using APICatalogo.Services;
using Microsoft.Extensions.Logging;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        //injeção de dependencias
        public CategoriasController(AppDbContext contexto, IConfiguration config, ILogger<CategoriasController> logger)
        {
            _context = contexto;
            _configuration = config;
            _logger = logger;
        }


        [HttpGet("connection")]
        public string GetConnection()
        {
            var conexao = _configuration["ConnectionStrings:DevConnection"];
            return $"Conexão: {conexao}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuService meuservice, string nome)
        {
            return meuservice.Saudacao(nome);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.AsNoTracking().ToList();
        }


        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {

            _logger.LogInformation("============================= GET api/categorias/produtos =================");
            return _context.Categorias.Include(x => x.Produtos).ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            //aumentando o desempenho com AsnoTracking, mas só podera ser usado quando não for fazer nenhuma alteração no objeto
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            //a partir da versão 3.0 não é necessario essa validação 
            //ele ja faz automaticamente porem tera que conter [ApiController]

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId) return BadRequest();
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            //var produto = _context.Categoria.Find(id)

            if (categoria == null) return NotFound();
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return categoria;

        }
    }
}
