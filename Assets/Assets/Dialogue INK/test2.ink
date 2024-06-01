INCLUDE global.ink
{ nama_equipment == "": -> main | -> already_chose}
-> main

=== main ===
kamu memilih apa?
    +[Pedang]
        -> chosen("Pedang")
    +[Kalung]
        -> chosen("Kalung")
    +[Pelindung]
        -> chosen("Pelindung")
=== chosen(test2) ===
~ nama_equipment = test2
kamu memilih {test2}!
-> END

=== already_chose ===
kamu sudah memilih {nama_equipment}!
-> END