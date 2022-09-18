using BlazorDapperApp.Data;

namespace BlazorDapperApp.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<int> Create(Produto produto);
        Task<int> Update(Produto produto);
        Task<int> Delete(int Id);
        Task<Produto> GetById(int Id);
        Task<List<Produto>> ListAll();
    }
}