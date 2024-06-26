# Обучающая программа по индексации в PostgreSQL

Этот проект представляет собой обучающую программу по темам "Простые, составные, частичные индексы и индексы с включёнными столбцами в PostgreSQL". Программа включает теоретический материал, а также эмуляцию B+ дерева для визуализации работы индексов.

## Содержание

- [Описание](#описание)
- [Функционал](#функционал)
- [Теоретический материал](#теоретический-материал)
- [Эмуляция B+ дерева](#эмуляция-b-дерева)
- [Требования](#требования)
- [Установка](#установка)
- [Установочный файл](#установочный-файл)

## Описание

Этот репозиторий содержит учебные материалы и инструменты для изучения различных типов индексов в PostgreSQL. Основное внимание уделяется следующим темам:

- Простые индексы
- Составные индексы
- Частичные индексы
- Индексы с включёнными столбцами

## Функционал

1. **Теоретический материал**:
   - Все теоретические материалы хранятся в формате HTML и содержат подробные объяснения и примеры по каждой из тем.
  
2. **Эмуляция B+ дерева**:
   - Реализована структура данных B+ дерева.
   - Создан элемент `drawbox` для визуализации B+ дерева.

## Теоретический материал

Теоретические материалы находятся в папке `Lessons` и включают следующие файлы:

- `introduction.html`: Введение.
- `scan.html`: Сканирование.
- `indexScan.html`: Индексное сканирование.
- `createIndex.html`: Создание индекса.
- `btree.html`: B-tree.
- `gist.html`: GiST.
- `hash.html`: Hash index.
- `simple.html`: Простые индексы.
- `complex.html`: Составные индексы.
- `partial.html`: Частичные  столбцами.
- `included.html`: Индексы с включёнными столбцами.
- `ending.html`: Заключение.

Каждый HTML файл содержит текстовые объяснения и примеры кода.

## Эмуляция B+ дерева

Код для эмуляции B+ дерева находится в папке `BTree`. Основные компоненты:

- `BTreeStructure`: Реализация структуры данных B+ дерева.
- `DrawBox`: Компонент для визуализации B+ дерева.

Эти инструменты позволяют увидеть, как данные хранятся и извлекаются с помощью индексов, реализованных на основе B+ дерева.

## Требования

- C# + WinForms
- Visual Studio 2022

## Установка

1. Клонируйте репозиторий:

    ```bash
    git clone https://github.com/goorwi/repo-name.git
    cd repo-name
    ```

2. Следуйте инструкциям на экране для взаимодействия с B+ деревом.

## Установочный файл

Установочный файл созданный при помощи Wix v4 содержится в корневой папке. Название: `Setup.msi`
