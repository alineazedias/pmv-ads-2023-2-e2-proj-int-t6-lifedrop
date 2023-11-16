﻿using LifeDrop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LifeDrop.Controllers
{
    public class MeuPerfilController : Controller
    {
        private readonly AppDbContext _context;
        private Usuario _usuarioLogado;

        public MeuPerfilController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;

            _usuarioLogado = _context.Usuarios.FirstOrDefault(x => x.Nome == identity.Name);

            var doador = _context.Doadores.FirstOrDefault(x => x.IdUsuario == _usuarioLogado.IdUsuario);

            if (doador == null)
            {
                doador = new Doador
                {
                    IdUsuario = _usuarioLogado.IdUsuario,
                    Nome = _usuarioLogado.Nome,
                    Email = _usuarioLogado.Email
                };
            }

            return View(doador);
        }

        public async Task<IActionResult> Salvar(int idDoador, Doador doador)
        {
            if (ModelState.IsValid)
            {
                if (doador.IdDoador > 0 && doador.IdDoador == idDoador)
                {
                    Doador doadorExistente = _context.Doadores.Find(idDoador);
                    doadorExistente.Nome = doador.Nome;
                    doadorExistente.Email = doador.Email;
                    doadorExistente.Telefone = doador.Telefone;
                    doadorExistente.CPF = doador.CPF;
                    doadorExistente.DataNasc = doador.DataNasc;
                    doadorExistente.Endereco = doador.Endereco;
                    doadorExistente.Genero = doador.Genero;
                    doadorExistente.TipoSanguineo = doador.TipoSanguineo;

                    _context.Doadores.Update(doadorExistente);

                    _usuarioLogado.Nome = doador.Nome;
                    _context.Usuarios.Update(_usuarioLogado);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    _context.Doadores.Add(doador);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        //public Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var dados = await _context.Agendamentos.FindAsync(id);

        //    if (dados == null)
        //        return NotFound();


        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(int id, Agendamento agendamento)
        //{
        //    if (id != Agendamento.Id)
        //        return NotFound();

        //    if (ModalState.IsValid)
        //    {
        //        _context.Agendamentos.Update(agendamento);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}

    }


}

