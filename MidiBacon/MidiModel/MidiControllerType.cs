namespace MidiBacon
{
    public enum MidiControllerType
    {
        BankSelect = 0x00   //0 (0x00)	Bank Select
                            //Modulation//1 (0x01)
                            //BreathController //2 (0x02)
                            //FootController//4 (0x04)
                            //5 (0x05)	PortamentoTime=
                            //6 (0x06)	DataEntryMSB=
                            //7 (0x07)	MainVolume=
                            //8 (0x08)	Balance=
                            //10 (0x0A)	Pan=
                            //11 (0x0B)	ExpressionController=
                            //12 (0x0C)	EffectControl1=
                            //13 (0x0D)	EffectControl2=
                            //16-19 (0x10-0x13)	GeneralPurposeControllers1To4=
                            //32-63 (0x20-0x3F)	LSBforcontrollers0To31=
                            //64 (0x40)	DamperPedalSustain=
                            //65 (0x41)	Portamento=
                            //66 (0x42)	Sostenuto=
                            //67 (0x43)	SoftPedal=
                            //68 (0x44)	LegatoFootswitch=
                            //69 (0x45)	Hold2=
                            //70 (0x46)	SoundController1= //(default: Timber Variation)
                            //71 (0x47)	SoundController2= //(default: Timber/Harmonic Content)
                            //72 (0x48)	SoundController3= //(default: Release Time)
                            //73 (0x49)	SoundController4= //(default: Attack Time)
                            //74-79 (0x4A-0x4F)	SoundController 6-10=
                            //80-83 (0x50-0x53)	GeneralPurposeControllers 5-8=
                            //84 (0x54)	PortamentoControl=
                            //91 (0x5B)	Effects1Depth=
                            //92 (0x5C)	Effects2Depth=
                            //93 (0x5D)	Effects3Depth=
                            //94 (0x5E)	Effects4Depth =
                            //95 (0x5F)	Effects5Depth=
                            //96 (0x60)	DataIncrement=
                            //97 (0x61)	DataDecrement=
                            //98 (0x62)	NonRegisteredParameterNumberLSB=
                            //99 (0x63)	NonRegisteredParameterNumberMSB=
                            //100 (0x64)	RegisteredParameterNumberLSB=
                            //101 (0x65)	RegisteredParameterNumberMSB=
                            //121-127 (0x79-0x7F)	ModeMessages=
    }
}