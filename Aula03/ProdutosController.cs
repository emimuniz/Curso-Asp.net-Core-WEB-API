using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        } 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
           return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            //aumentando o desempenho com AsnoTracking, mas só podera ser usado quando não for fazer nenhuma alteração no objeto
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            //a partir da versão 3.0 não é necessario essa validação 
            //ele ja faz automaticamente porem tera que conter [ApiController]

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);

        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest();
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(id)

            if (produto == null) return NotFound();
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return produto;

        }
    }
}
