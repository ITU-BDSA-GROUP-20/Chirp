using Chirp.CLI;
using SimpleDB;

static class UserInterface {

    void PrintCheeps(IDatabaseRepository<Cheep> cheepDB, int amountOfMessages)
    {
        /*
            The cheep should be printed here, which means that we need the read method to return cheeps
            where we easily can use the .toString

        */
        cheepDB.Read(amountOfMessages);
    }
}