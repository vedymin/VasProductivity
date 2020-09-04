# VasProductivity

Jest to część projektu mierzenia produktywności na dziale VAS. Program napisany w C# pozwala wybrać stanowisko pracy (np biurko PACK003) i skanować ID boxów.
Każdy skan jest zapisywany w bazie MySQL, wraz z datą i godziną, stanowiskiem, ilością przedmiotów w boxie, oraz kodami aktywności VAS. Dwa ostatnie parametry pobierane są bezpośrednio z bazy danych Reflex (System magazynowy). Program działa bez problemów na >40 stanowiskach. Sprawdza czy dany box był już zeskanowany - jesli tak pokazuje na którym stanowisku i o której godzinie. 

Do interpretowania danych służy aplikacja https://github.com/vedymin/Vas-Dashboard
