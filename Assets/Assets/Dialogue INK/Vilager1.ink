INCLUDE global.ink

Halo Mahapatih GajahMada! #speaker:Villager #potrait:vilager_default #layout:left #audio:drawKnife1
-> main

=== main ===
apakah tuan baru disini?
+ [Ya]
  ~ playEmote("exclamation")
    Ah begitu.. jadi tuan harus mengalahkan semua musuh di dalam stage ini lalu disana terdapat gubuk kecil yang dimana untuk menyelesaikan stage ini dengan menyelesaikan beberapa quiz tersebut. #potrait:vilager_default
+ [Tidak]
    ah kalau begitu langsung saja bermain tuan! #potrait:villager_sad #layout:left
        ->TidakAnswer
    
- baiklah saya mengerti. #speaker:GajahMada #potrait:gajahmada_neutral #layout:right

~ playEmote("question")
apakah ada pertanyaan yang lain, tuan? #speaker:Villager #potrait:vilager_default #layout:left

+ [Ya]
    -> tutorial
+ [Tidak]
    ~ playEmote("exclamation")
    baiklah kalau begitu, silahkan bermain! #audio:drawKnife3
    -> END
    
=== TidakAnswer ====
-> END
=== tutorial ===
test saja kawan #audio:drawKnife2
-> DONE

