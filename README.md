# Pacman Game

## Popis hry
Pacman je klasická arkádová hra, ve které hráč ovládá postavičku Pacmana, která se snaží jíst všechny body (mince) na herní ploše a vyhýbat se duchům.

## Použité knihovny
- OptimizedPriorityQueue: Tato knihovna je použita k implementaci A* algoritmu pro hledání cesty duchů k Pacmanovi.
- System.Data.SqlClient: Tato knihovna je použita k komunikaci s databází pro ukládání výsledků hráčů.

## Funkce hry
- Hráč ovládá postavu Pacmana pomocí kláves WASD.
- Cílem hry je jíst co nejvíce mincí na herní ploše a vyhýbat se duchům.
- Mince přidávají skóre které se nachází v levém horním rohu.
- První duch (červený) jde okamžitě po vás a další duchové vstupují do hry po 7s.
    1. Červený
    2. Růžový
    3. Modrý
    4. Oranžový
- Pacman má tři životy úkázané v pravém horním rohu.
- Hra končí, když Pacman ztratí všechny životy.
- Poté si hráč může vybrat jestli si chce uložit svoje skóre do tabulky (napíše svoje jméno a klikne na tlačítko save) nebo restartovat hru (pomocí klávesy R) a začít znovu.
- Leaderboard neboli tabulka která ukazuje ty nejlepší hráče.

## Instalace a spuštění
1. Stažení nebo naklonování repozitáře z GitHubu.
2. Otevření projektu v IDE (Visual Studio, Visual Studio Code).
3. Zajištění, že jsou nainstalovány všechny potřebné závislosti (OptimizedPriorityQueue, System.Data.SqlClient).
4. Kompilace a spuštění projektu.
    nebo
1. Spuštění EXE souboru s názvem PacMan.

## Příklad použití A* algoritmu
Pro implementaci A* algoritmu pro hledání cesty duchů k Pacmanovi je použita knihovna OptimizedPriorityQueue. Tato knihovna umožňuje efektivně implementovat prioritní frontu potřebnou pro A* algoritmus.

## Připojení k databázi
Pro ukládání výsledků hráčů je využita knihovna System.Data.SqlClient od Microsoftu. Tato knihovna umožňuje komunikaci s relační databází (např. SQL Server) a provádění databázových operací jako je vkládání, aktualizace nebo dotazování dat.

