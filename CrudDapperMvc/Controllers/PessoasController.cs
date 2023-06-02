using CrudDapperMvc.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CrudDapperMvc.Controllers
{
    public class PessoasController : Controller
    {
        private readonly string connectionsString = "Server=DESKTOP-O1775Q7;Database=PessoasDb;User Id=sa;Password=123456;Trusted_Connection=True;TrustServerCertificate=True;";


        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Pessoas> listaPessoas;
            IDbConnection con;

            try
            {
                string query = "SELECT * FROM Pessoas";
                con = new SqlConnection(connectionsString);
                con.Open();
                listaPessoas = con.Query<Pessoas>(query).ToList();
                con.Close();
                return View(listaPessoas);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pessoas pessoas)
        {
            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    string query = "INSERT INTO Pessoas(Nome, Idade, Peso) VALUES(@Nome, @Idade, @Peso)";
                    con = new SqlConnection(connectionsString);
                    con.Open();
                    con.Execute(query, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return View(pessoas);
        }

        [HttpGet]
        public IActionResult Edit(int pessoaId)
        {
            if (pessoaId == null) return NotFound();
            IDbConnection con;

            try
            {
                string query = "SELECT * FROM pessoas WHERE PessoaId = @pessoaId";
                con = new SqlConnection(connectionsString);
                con.Open();
                Pessoas pessoas = con.Query<Pessoas>(query, new { pessoaId = pessoaId}).FirstOrDefault();
                con.Close();
                return View(pessoas);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Edit(int pessoaId, Pessoas pessoas)
        {
            if (pessoaId != pessoas.PessoaId) return NotFound();

            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    con = new SqlConnection(connectionsString);
                    string queryAtualizada = "UPDATE pessoas SET Nome=@Nome, Idade=@Idade, Peso=@Peso WHERE PessoaId=@pessoaId";
                    con.Open();
                    con.Execute(queryAtualizada, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return View(pessoas);
        }

        [HttpPost]
        public IActionResult Delete(int pessoaId)
        {
                IDbConnection con;

                try
                {
                    con = new SqlConnection(connectionsString);
                    string query = "DELETE FROM pessoas WHERE PessoaId=@pessoaId";
                    con.Open();
                    con.Execute(query, new {pessoaId = pessoaId});
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    throw ex;
                }
        }
    }
}
