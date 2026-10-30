using Domain.Exceptions;


namespace Presentation.Input
{
    public static class Parser
    {

        public static List<long> ParseStringToLong(string[] arr) 
        {
            var res = new List<long>();
            
            for (var ind = 0; ind < arr.Length; ind++)
            {
                if (long.TryParse(arr[ind], out long id))
                {
                    res.Add(id);
                }
                else
                {
                    throw new WrongInputException($"Некорректное значение ClientId - {arr[ind]}");
                }

            }
            return res;
        }
    }
}
