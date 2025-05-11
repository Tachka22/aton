using aton.domain.Entities;

namespace aton.domain.Interfaces;

public interface IUserRepository
{
    //Create
    Task<User> Create(User user);

    //Read
    Task<IEnumerable<User>> GetAllActive();
    Task<User> GetByLogin(string login);
    Task<IEnumerable<User>> GetAllByAge(DateTime minDateTime);


    //Update
    void Update(User user);
}
