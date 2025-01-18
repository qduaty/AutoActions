namespace core
{
    extern "C" void  SetGlobalHDR(bool enabled);
    class HDRController
    {
    public:
        static void SetHDR(bool enabled);
        static bool HDRIsOn();
    };

}

