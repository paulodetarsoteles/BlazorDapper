using BlazorDapperApp.Data;
using BlazorDapperApp.Services.Interfaces;
using Dapper;
using System.Data;

namespace BlazorDapperApp.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IDapperService _dapperDAL;

        public ProdutoService(IDapperService dapperDAL)
        {
            _dapperDAL = dapperDAL;
        }

        public Task<List<Produto>> ListAll()
        {
            var produtos = Task.FromResult(_dapperDAL.GetAll<Produto>
                (
                    $"SELECT * FROM [Produtos]",
                    null, commandType: CommandType.Text
                ));
            return produtos;
        }

        public Task<Produto> GetById(int id)
        {
            var produto = Task.FromResult(_dapperDAL.Get<Produto>
                (
                    $"SELECT * FROM [Produtos]" + 
                    $"WHERE ProdutoId = {id}", 
                    null, commandType: CommandType.Text
                )); 
            return produto;
        }

        public Task<int> Create(Produto produto)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Estoque", produto.Estoque, DbType.Int32);
            dbPara.Add("Nome", produto.Nome, DbType.String);
            dbPara.Add("Preco", produto.Preco, DbType.Decimal);
            dbPara.Add("Descricao", produto.Descricao, DbType.String);

            var produtoId = Task.FromResult
                (
                    _dapperDAL.Insert<int>("[dbo].[SP_Novo_Produto]", dbPara, commandType: CommandType.StoredProcedure)
                );

            return produtoId; 
        }

        public Task<int> Update(Produto produto)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ProdutoId", produto.ProdutoId);
            dbPara.Add("Estoque", produto.Estoque, DbType.Int32);
            dbPara.Add("Nome", produto.Nome, DbType.String);
            dbPara.Add("Preco", produto.Preco, DbType.Decimal);
            dbPara.Add("Descricao", produto.Descricao, DbType.String);

            var updateProduto = Task.FromResult
                (
                    _dapperDAL.Update<int>("[dbo].[SP_Atualiza_Produto]", dbPara, commandType: CommandType.StoredProcedure)
                );

            return updateProduto;
        }

        public Task<int> Delete(int id)
        {
            var deleteProduto = Task.FromResult(_dapperDAL.Execute
                (
                    $"DELETE [Produtos]" +
                    $"WHERE ProdutoId = {id}",
                    null, commandType: CommandType.Text
                ));
            return deleteProduto;
        }
    }
}