namespace Day03Davide{
    /// <summary>
    /// Diese Klasse ist sehr grusig
    /// </summary>
    public class EngineSchematicsReader{
        private char[,] EngineSchema = new char[0,0];

        public List<SchemaNumber> SchemaNumbers = [];
        public List<Gear> Gears = [];

        private int Width;
        private int Height;

        public long SumValidSchemaNumbers = 0;
        public long SumGears = 0;

        public EngineSchematicsReader(string[] lines){
            CreateEngineSchema(lines);
            IdentifyNumbersInSchema();
            IdentifyValidNumbers();
            IdentifyGears();
            SumUpGears();
        }

        private void CreateEngineSchema(string[] lines){
            EngineSchema = new char[lines.Length, lines[0].Length];
            Height = EngineSchema.GetLength(0) - 1;
            Width = EngineSchema.GetLength(1) - 1;
            var heightCount = 0;
            foreach (var line in lines)
            {
                for (int i = 0; i < Width + 1; i++)
                {
                    EngineSchema[heightCount, i] = line[i];
                }
                heightCount++;
            }

        }

        private void IdentifyNumbersInSchema(){
            for (int row = 0; row < Height + 1; row++)
            {
                string number = string.Empty;
                int? startIndex = null;
                for (int column = 0; column < Width + 1; column++)
                {
                    char c = EngineSchema[row, column];
                    
                    if (char.IsDigit(c)){
                        number += c;
                        startIndex = !startIndex.HasValue ? column : startIndex;
                    }
                    if ((!char.IsDigit(c) || column == Width) && !string.IsNullOrEmpty(number)){

                        SchemaNumbers.Add(new (int.Parse(number), row, startIndex.HasValue ? (int)startIndex : 0, column == Width ? column : column - 1));
                        number = string.Empty;
                        startIndex = null;
                    }
                }
            }
        }

        private void IdentifyValidNumbers(){
            foreach (var schemaNumber in SchemaNumbers)
            {
                SumValidSchemaNumbers += IsValid(schemaNumber) ? schemaNumber.Value : 0;
            }
        }

        private bool IsValid(SchemaNumber schemaNumber){
            int startIndex = schemaNumber.StartIndex == 0 ? 0 : schemaNumber.StartIndex - 1;
            int endIndex = schemaNumber.EndIndex == Width ? Width : schemaNumber.EndIndex + 1;
            int topIndex = schemaNumber.Row == 0 ? 0 : schemaNumber.Row - 1;
            int bottomIndex = schemaNumber.Row == Height ? Height : schemaNumber.Row + 1;
            for (int row = topIndex; row < bottomIndex + 1; row++)
            {
                for (int column = startIndex; column < endIndex + 1; column++)
                {
                    char c = EngineSchema[row, column];
                    if (!char.IsLetterOrDigit(c) && c != '.'){
                        return true;
                    }
                }
            }
            return false;
        }

        private void IdentifyGears(){
            for (int row = 0; row < Height + 1; row++)
            {
                for (int column = 0; column < Width + 1; column++)
                {
                    char c = EngineSchema[row, column];
                    
                    if (c == '*'){
                        Gears.Add(new Gear(row, column, Height, Width));
                    }
                
                }
            }
        }

        /// <summary>
        /// Sehr grusig
        /// </summary>
        /// <returns></returns>
        public void SumUpGears(){
            foreach (var gear in Gears)
            {
                List<long> numbers = new();
                foreach (var schemaNumber in SchemaNumbers)
                {
                    foreach (var field in schemaNumber.Fields)
                    {
                        if (gear.Fields.Where(g => field == g).Any()){
                            numbers.Add(schemaNumber.Value);
                            break;
                        }
                    }
                }

                long sum = 0;
                if (numbers.Count > 1){
                    for (int i = 0; i < numbers.Count - 1; i++)
                    {
                        for (int j = i + 1; j < numbers.Count; j++)
                        {
                            sum += numbers[i] * numbers[j];
                        }
                    }                
                }
                SumGears += sum;
            }
        }

    }
}