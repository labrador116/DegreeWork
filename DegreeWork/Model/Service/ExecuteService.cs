﻿using DegreeWork.ChromosomeModel;
using DegreeWork.Container;
using DegreeWork.SpaceParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DegreeWork.Service
{
    public class ExecuteService
    {
        private List<int> _radiusContainer;
        private Population _populationContainer;
        private List<ResultModel> _result;
        private static ParallelOptions parOps = new ParallelOptions();
        
        public event EventHandler WrongParams;
        public delegate void SendPopulation (Chromosome chromosome);
        public event SendPopulation callback;

        public ExecuteService (List<int> container)
        {
            _radiusContainer = container;
        }

        private static int getRandomValue(int from, int to)
        {
            Thread.Sleep(100);
            return new Random().Next(from, to);
        }

        public static void RefactorBadGene(Gene gene)
        {
            gene.OX = getRandomValue(0, SingleSpaceParams.getInstance().Width);
            gene.OY = getRandomValue(0, SingleSpaceParams.getInstance().Height);  
        }

        public void Start()
        {
            _populationContainer = new Population();
            parOps.MaxDegreeOfParallelism = Environment.ProcessorCount;
            Executing();
        }

        public void Executing()
        {
            /*Создание первой популяции*/
            createFirstPopulation();
            
            _result = new List<ResultModel>();
            int counter = 0;
            while (true)
            {
                //Условия оценки первой популяции
                if (counter == 0)
                {
                    //Оценивание хромосом
                   foreach(Chromosome chr in _populationContainer.GetSetPopulationContainer)
                     {
                         ResultModel resM = new ResultModel();

                         resM.Ratio = GeneticAlgorithm.GA.EvaluationOfFitenssFunc(chr);
                         resM.Chromosome = chr;
                         
                         _result.Add(resM);
                     }
                    SingleSpaceParams.getInstance().GlobalResultContainerGetSet.Add(_result.ElementAt(0));
                }
                else
                {
                    //Кодирование всех хромосом
                    //Parallel.ForEach(_populationContainer.GetSetPopulationContainer, parOps, chr =>
                    foreach(Chromosome chr in _populationContainer.GetSetPopulationContainer)
                    {
                        GeneticAlgorithm.GA.GA_Encode(chr);
                    }
                   
                    //Селекция
                    _result.Sort((a, b) => b.Ratio.CompareTo(a.Ratio));

                    SingleSpaceParams.getInstance().GlobalResultContainerGetSet.Add(_result.ElementAt(0));

                    //Выход из алгоритма
                    if (SingleSpaceParams.getInstance().NumOfPopulation != -1)
                    {
                        if (counter == SingleSpaceParams.getInstance().NumOfPopulation)
                        {
                            break;
                        }
                    }
                    if (SingleSpaceParams.getInstance().CriterionOfQuality != -1)
                    {
                        if(_result.ElementAt(0).Ratio >= SingleSpaceParams.getInstance().CriterionOfQuality)
                        {
                            break;
                        }
                    }
                    if(SingleSpaceParams.getInstance().TheBestResolve != -1)
                    {
                        int countAccessResult= 0;
                        for (int i = 1; i < SingleSpaceParams.getInstance().GlobalResultContainerGetSet.Count; i++)
                        {
                            ResultModel result = SingleSpaceParams.getInstance().GlobalResultContainerGetSet.ElementAt(i);

                            if(result.Ratio == SingleSpaceParams.getInstance().GlobalResultContainerGetSet.ElementAt(i - 1).Ratio)
                            {
                                countAccessResult++;
                            }
                            else
                            {
                                countAccessResult = 0;
                            }

                            if (countAccessResult == 5)
                            {
                                callback(_result.ElementAt(0).Chromosome);
                                return;
                            }
                        }
                    }

                    //Вычисляем кол-во родителей
                    int count;
                    if ((_result.Count / 2) % 2 == 0)
                    {
                        count = _result.Count / 2;
                    }
                    else
                    {
                        count = (_result.Count / 2) + 1;
                    }

                    //Список хромосом для Кроссинговера
                    List<Chromosome> listForSelection = new List<Chromosome>();
                    //Parallel.For(0, count, i =>
                    for(int i =0;i<count;i++)
                     {
                         listForSelection.Add(_result.ElementAt(i).Chromosome);
                     }
                    

                    //Кроссинговер
                    GeneticAlgorithm.GA.CrossingOver(listForSelection);

                    _populationContainer = new Population();

                    //Мутация и размещение в контейнере 
                  //  Parallel.ForEach(listForSelection, parOps, chr =>
                  foreach(Chromosome chr in listForSelection)
                    {
                        GeneticAlgorithm.GA.Mutation(chr);
                        _populationContainer.GetSetPopulationContainer.Add(chr);
                    }

                    //ToDo мутация
                    _result = new List<ResultModel>();
                    //Декодирование и оценивание всех хромосом
                    //Parallel.ForEach(_populationContainer.GetSetPopulationContainer, parOps, chr =>
                    foreach(Chromosome chr in _populationContainer.GetSetPopulationContainer)
                     {
                         GeneticAlgorithm.GA.GA_Decode(chr);

                         ResultModel resM = new ResultModel();
                         resM.Ratio = GeneticAlgorithm.GA.EvaluationOfFitenssFunc(chr);
                         resM.Chromosome = chr;

                         _result.Add(resM);
                     }
                    
                }
                counter++;
            }
            callback(_result.ElementAt(0).Chromosome);
        }

        private void createFirstPopulation()
        {
            /*Создаем популяцию из 10 хромосом. Каждая гена имеет уникальное размещение. */
            for (int i = 0; i < 10; i++)
            {
                Chromosome chromosome = new Chromosome();
                int countOfPosition = 0;
                foreach (int item in _radiusContainer)
                {
                    Gene gene = new Gene(item,
                        getRandomValue(0, SingleSpaceParams.getInstance().Width),
                        getRandomValue(0, SingleSpaceParams.getInstance().Height),
                        countOfPosition
                        );

                    chromosome.Container.Add(gene);
                    countOfPosition++;
                }
                GeneticAlgorithm.GA.CheckIntersection(chromosome);
                _populationContainer.GetSetPopulationContainer.Add(chromosome);
            }
            
            /*Хромосома с допустимыми координатами размещения*/
            /*
            Chromosome chromosome = _populationContainer.GetSetPopulationContainer.ElementAt(0);
            Coordinate coords;
            List<Coordinate> coordsList = new List<Coordinate>();
            foreach (Gene gene in chromosome.Container)
            {
                coords = new Coordinate();
                coords.CoordX = gene.OX;
                coords.CoordY = gene.OY;
                coords.Position = gene.NumOfPosition;
                coordsList.Add(coords);
            }*/
        }
    }
}
