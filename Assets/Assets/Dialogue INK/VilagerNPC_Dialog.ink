INCLUDE global.ink

Selamat datang, Gajah Mada! Kamu baru saja memasuki dunia kami yang penuh petualangan! #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
* [Halo] Apakah anda sudah mengerti memainkan ini? #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
-> main

=== main ===
+[ Tidak]
    ~ playEmote("exclamation")
    Baiklah, sebelum kamu melangkah lebih jauh, ada beberapa hal yang perlu kamu ketahui. Pertama, di dunia ini terdapat berbagai macam misi dan pencapaian yang dapat kamu raih.  #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
    -> continue2
    = continue2
    + [Lalu?]
        Anda bisa menggerakan pemain menggunakan keyboard yaitu A dan D untuk berjalan #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
        -> continue3
        =continue3
        + [Cara Menyerang?]
        ~ playEmote("exclamation")
        oh iya! untuk cara menyerang menekan tombol J, itu gerakan dasar untuk sebagai pemain baru pada permainan ini. #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
        -> Paham

=== Paham ===
Apakah anda sudah paham? #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
+ [Paham]
-> PahamContinue
+ [Tidak]
-> TidakPaham
= PahamContinue
~ playEmote("exclamation")
baiklah lanjutkan petualangan anda sebagai Gajah Mada! #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
-> END
= TidakPaham
ah Baiklah saya akan mengulanginya. #speaker:Villager #potrait:vilager_default #layout:right #audio:drawKnife1
+ [Baiklah]
-> main



