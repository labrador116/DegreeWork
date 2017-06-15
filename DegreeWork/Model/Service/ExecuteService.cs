using DegreeWork.ChromosomeModel;
using DegreeWork.Container;
using DegreeWork.SpaceParam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        BackgroundWorker _BackgroundWorker;

        public ExecuteService (List<int> container, BackgroundWorker backgroundWorker)
        {
            _radiusContainer = container;
            _BackgroundWorker = backgroundWorker;
        }

        private static int getRandomValue(int from, int to)
        {
            Thread.Sleep(200);
            return new Random().Next(from, to);
        }

        public static void RefactorBadGene(Gene gene)
        {
            gene.OX = getRandomValue(0, SingleSpaceParams.getInstance().Width);
            gene.OY = getRandomValue(0, SingleSpaceParams.getInstance().Height);  
        }
        public static void RefactorBadGeneForWidthFrom(Gene gene, int from)
        {
            gene.OX = getRandomValue(from, SingleSpaceParams.getInstance().Width);
            gene.OY = getRandomValue(0, SingleSpaceParams.getInstance().Height);
        }
        public static void RefactorBadGeneForWidthTo(Gene gene,int to)
        {
            gene.OX = getRandomValue(0, to);
            gene.OY = getRandomValue(0, SingleSpaceParams.getInstance().Height);
        }

        public Chromosome Start()
        {
            _populationContainer = new Population();
            parOps.MaxDegreeOfParallelism = Environment.ProcessorCount;
            _BackgroundWorker.ReportProgress(0, "Старт");
            return Executing();
        }

        public Chromosome Executing()
        {
            /*Создание первой популяции*/
            _BackgroundWorker.ReportProgress(0, "Создание первой популяции");
            createFirstPopulation();
            
            _result = new List<ResultModel>();
            int counter = 0;
            while (true)
            {
                //Условия оценки первой популяции
                if (counter == 0)
                {
                    //Оценивание хромосом
                    _BackgroundWorker.ReportProgress(20, "Оцениваем полученныые решения");
                    foreach (Chromosome chr in _populationContainer.GetSetPopulationContainer)
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
                    _BackgroundWorker.ReportProgress(30, "Началась селекция");
                    //Селекция
                    _result.Sort((a, b) => b.Ratio.CompareTo(a.Ratio));

                    SingleSpaceParams.getInstance().GlobalResultContainerGetSet.Add(_result.ElementAt(0));

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

                            if (countAccessResult == 5 || result.Ratio == 1)
                            {
                                _BackgroundWorker.ReportProgress(100, "Готово");
                                return _result.ElementAt(0).Chromosome;
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
                   
                    for(int i =0;i<count;i++)
                     {
                         listForSelection.Add(_result.ElementAt(i).Chromosome);
                     }

                    _BackgroundWorker.ReportProgress(50, "Началась операция кроссинговера");
                    //Кроссинговер
                    GeneticAlgorithm.GA.CrossingOver(listForSelection);

                    _populationContainer = new Population();

                    _BackgroundWorker.ReportProgress(60, "Началась операция мутации");
                    //Мутация и размещение в контейнере 
                    foreach (Chromosome chr in listForSelection)
                    {
                       // GeneticAlgorithm.GA.Mutation(chr);
                        _populationContainer.GetSetPopulationContainer.Add(chr);
                    }

                    //ToDo мутация
                    _result = new List<ResultModel>();

                    _BackgroundWorker.ReportProgress(80, "Оцениваем результаты");
                    //Оценивание всех хромосом
                    foreach (Chromosome chr in _populationContainer.GetSetPopulationContainer)
                     {
                         ResultModel resM = new ResultModel();
                         resM.Ratio = GeneticAlgorithm.GA.EvaluationOfFitenssFunc(chr);
                         resM.Chromosome = chr;

                         _result.Add(resM);
                     }
                }
                counter++;
            }
        }

        private void createFirstPopulation()
        {
            /*Создаем популяцию из 10 хромосом. Каждая гена имеет уникальное размещение. */
            for (int i = 0; i < SingleSpaceParams.getInstance().SumOfChromosomeInPopulation; i++)
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
                _BackgroundWorker.ReportProgress(10, "Создана " + (i+1) + "-я хромосома в первой популяции");
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
