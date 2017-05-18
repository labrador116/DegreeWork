using GeneticAlgorithmCourseWork.ChromosomeModel;
using GeneticAlgorithmCourseWork.Container;
using GeneticAlgorithmCourseWork.Service;
using GeneticAlgorithmCourseWork.SpaceParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithmCourseWork.GeneticAlgorithm
{
   public class GA
    {
        /*Проверка всех окружностей на занимаемую плоскость*/
        public static bool CheckToArea(Chromosome chromosome)
        {
            double sumCirclesArea = 0;

            List<Gene> genes = chromosome.Container;

            foreach (Gene item in genes)
            {
                sumCirclesArea += Math.PI * (item.Radius * item.Radius);
            }

            if ((SingleSpaceParams.getInstance().Width * SingleSpaceParams.getInstance().Height) / sumCirclesArea < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*Проверка пересечений*/
        public static bool CheckIntersection(Chromosome chromosome, int val)
        {
            int length = chromosome.Container.Count;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (j != i)
                    {
                        bool resultValue = AlgorithmOfCheckIntersection(
                            chromosome.Container.ElementAt(i),
                            chromosome.Container.ElementAt(j),
                            chromosome.AreaWidth,
                            chromosome.AreaHeight
                            );

                        if (resultValue == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /*Проверка пересечений*/
        public static void CheckIntersection(Chromosome chromosome)
        {
            int length = chromosome.Container.Count;

            for (int i = 0; i <length ; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (j != i)
                    {
                        bool resultValue = AlgorithmOfCheckIntersection(
                            chromosome.Container.ElementAt(i),
                            chromosome.Container.ElementAt(j),
                            chromosome.AreaWidth,
                            chromosome.AreaHeight
                            );

                        if (resultValue == true )
                        {
                            ExecuteService.RefactorBadGene(chromosome.Container.ElementAt(i));
                            j = -1;
                        }
                    }
                }
            }
        }  
        private static bool AlgorithmOfCheckIntersection(Gene A, Gene B, int width, int height)
        {
            double radiusLenght=Math.Sqrt(
                ((A.OX - B.OX)*(A.OX - B.OX))+
                ((A.OY-B.OY)*(A.OY - B.OY))
                );
            /*
             * Данное условие провярет на:
             * -Пересечение двух окружностей
             * -Размещение окружностей относительно границ плоскости
             * -Размещение одной окружности внутри другой окружности
             */
             //ToDo проверка на размеры плоскости индивидуально по хромосоме
            if (
                (Convert.ToInt32(radiusLenght) > A.Radius + B.Radius) &&
                !(Convert.ToInt32(radiusLenght) < A.Radius - B.Radius) &&
                ((A.OX-A.Radius)>=0 && (A.OX+A.Radius <= width)) &&
                ((A.OY-A.Radius)>=0 && (A.OY+A.Radius <= height))
               )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /*Корректировка ширины плоскасти*/
        private static void AdjustmentOfAreA(Chromosome chromosome)
        {
            List<int> distanceToWeightBorder = new List<int>();
            List<int> distanceToHeghtBorder = new List<int>();

            foreach (Gene gene in chromosome.Container)
            {
                 distanceToWeightBorder.Add(chromosome.AreaWidth - (gene.OX + gene.Radius));
                distanceToHeghtBorder.Add(chromosome.AreaHeight-(gene.OY + gene.Radius));
            }

            chromosome.AreaWidth-=distanceToWeightBorder.Min();
            chromosome.AreaHeight-=distanceToHeghtBorder.Min();
        }

        /*Оценка фитнесс-функции*/
        public static double EvaluationOfFitenssFunc(Chromosome chromosome)
        {
            AdjustmentOfAreA(chromosome);

            double sumOfGenesArea = 0;

            foreach (Gene gene in chromosome.Container)
            {
                sumOfGenesArea += Math.PI * (gene.Radius * gene.Radius);
            }

            return (sumOfGenesArea / (chromosome.AreaWidth * chromosome.AreaHeight));
        }
        /*Кодирование позиции размещения*/
        public static void GA_Encode(Chromosome chromosome)
        {
            foreach (Gene gene in chromosome.Container)
            {
                String binValue = Convert.ToString(gene.NumOfPosition, 2);
                while (binValue.Length != 4)
                {
                    binValue = "0" + binValue;
                }

                gene.EncodeValue = binValue;
            }
        }

        public static void GA_Decode(Chromosome chromosome)
        {
            foreach (Gene gene in chromosome.Container)
            {
                if (gene.NumOfPosition == Convert.ToInt32(gene.EncodeValue,2))
                {
                    gene.EncodeValue = null;
                }
            }
        }

        //Кроссинговер
        public static void CrossingOver(List<Chromosome> chromosomesContainer)
        {
            int sumChromosome = chromosomesContainer.Count;

            for (int i=0;i<sumChromosome; i = i + 2)
            {
                int dotOfCrossingOver = new Random().Next(0, chromosomesContainer.ElementAt(i).Container.Count);

                Chromosome chrA = chromosomesContainer.ElementAt(i);
                Chromosome chrB = chromosomesContainer.ElementAt(i + 1);
                //Создание потомков
                Chromosome child_one = OperationCO(chrA, chrB, dotOfCrossingOver);
                Chromosome child_two = OperationCO(chrB, chrA, dotOfCrossingOver);

                //Устранение недопустимостей
                checkInvalid(child_one, chrA, chrB);
                checkInvalid(child_two, chrB, chrA);

                //Устранение незаконности
                checkLegalDecision(child_one, chrA, chrB);
                checkLegalDecision(child_two, chrB, chrA);

                //Добавление потомков в контейнер
                chromosomesContainer.Add(child_one);
                chromosomesContainer.Add(child_two);
            }
        }

         //Операция кроссинговера
        private static Chromosome OperationCO(Chromosome chromA, Chromosome chromB, int dotOfCross)
        {
            Chromosome childChromosome = new Chromosome();

            for (int i = 0; i< chromA.Container.Count; i++)
            {
                if (i <= dotOfCross)
                {
                    childChromosome.Container.Add(
                        chromA.Container.ElementAt(i)
                        );
                }
                if (i > dotOfCross)
                {
                    childChromosome.Container.Add(
                        chromB.Container.ElementAt(i)
                        );
                }
            }

            return childChromosome;
        }

        //Проверка на недопустимость
        private static void checkInvalid(Chromosome child_one, Chromosome chrA, Chromosome chrB)
        {
            object resultIsLegitimate = isInvalid(child_one);

            while (resultIsLegitimate != null)
            {
                int value = (int)resultIsLegitimate;

                child_one.Container.RemoveAt(value);
                child_one.Container.Insert(value, chrA.Container.ElementAt(value));

                resultIsLegitimate = isInvalid(child_one);

                if (resultIsLegitimate != null && value == (int)resultIsLegitimate)
                {
                    foreach (Gene gene in chrB.Container)
                    {
                        if (child_one.Container.Contains(gene) == false)
                        {
                            child_one.Container.RemoveAt(value);
                            child_one.Container.Insert(value, gene);
                            break;
                        }
                    }
                }
                resultIsLegitimate = isInvalid(child_one);
            }
        }

        private static object isInvalid (Chromosome chromosome)
        {
            for (int i=0; i < chromosome.Container.Count; i++)
            {
                for (int j =0; j < chromosome.Container.Count; j++)
                {
                    if (i != j)
                    {
                        if (chromosome.Container.ElementAt(i).EncodeValue == chromosome.Container.ElementAt(j).EncodeValue)
                        {
                            return j;
                        }
                    }
                }
            }
            return null ;
        }

        //Проверка на законность решения. Если нет, то создается новая хромосома.
        private static void checkLegalDecision(Chromosome child, Chromosome parentA, Chromosome parentB)
        {
            while (CheckIntersection(child, 0) != false)
            {
                Thread.Sleep(100);
                int dotOfCrossingOver = new Random().Next(0, parentA.Container.Count);
                child = OperationCO(parentA, parentB, dotOfCrossingOver);
                checkInvalid(child, parentA, parentB);
            }

        }

        public static void Mutation(Chromosome chr)
        {
            var random = new Random();
            
            if (random.NextDouble() < SingleSpaceParams.getInstance().PropabilityOfMutation)
            {
                while (CheckIntersection(chr, 0) != false)
                {
                    int breakPoint = new Random().Next(0, chr.Container.Count - 1);
                    List<Gene> genesMove = new List<Gene>();
                    int counter = 0;
                    foreach (Gene gene in chr.Container)
                    {
                        if (counter < breakPoint)
                        {
                            genesMove.Add(gene);
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    foreach(Gene gene in genesMove)
                    {
                        chr.Container.Remove(gene);
                        chr.Container.Add(gene);
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
