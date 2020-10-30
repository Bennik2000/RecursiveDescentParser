namespace StateMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            var validInput = "event switch_on, switch_off, timer;\n" +
                             "in initial state OFF:                           \n" +
                             "    on switch_on goto ON: turn_on_light, set_timer_5min;\n" +
                             "in state ON:                           \n" +
                             "    on switch_off, timer goto OFF: turn_off_light;\n";


            var parser = new Parser(validInput);

            var result = parser.Parse();
        }
    }
}
