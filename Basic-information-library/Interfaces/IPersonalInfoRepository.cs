using Basic_information_library.Models;


namespace Basic_information_library.Interfaces
{
    public interface IPersonalInfoRepository
    {
        void Save(PersonalInfo info);
    }
}

