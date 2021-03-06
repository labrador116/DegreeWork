﻿using DegreeWork.Instances;
using DegreeWork.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataBaseStruct.Model;
using DataBaseStruct;
using DegreeWork.Model.Instances;

namespace DegreeWork.SpaceParam
{
   public class SingleSpaceParams
    {
        private static SingleSpaceParams instance;
        private int _width;
        private int _height;
        private double _criterionOfQuality;
        private int _numOfPopulation;
        private int _theBestResolve;
        private double _propabilityOfMutation;
        private int sumOfChromosomeInPopulation;
        private List<ResultModel> GlobalResultContainer = new List<ResultModel>();
        private List<RectangleRoom> _rooms = new List<RectangleRoom>();
        private List<ControlPointInst> _controlPoints = new List<ControlPointInst>();
        private List<ModelsOfModules> _models = new List<ModelsOfModules>();
        private List<int> _modulesRadius = new List<int>();
        private List<double> _ratingByOrder = new List<double>();

        private SingleSpaceParams(int width, int height)
        {
            _width = width;
            _height = height;
            _criterionOfQuality = -1;
            _numOfPopulation = -1;
            _theBestResolve = -1;
        }

        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }
        public double CriterionOfQuality { get => _criterionOfQuality; set => _criterionOfQuality = value; }
        public int NumOfPopulation { get => _numOfPopulation; set => _numOfPopulation = value; }
        public double PropabilityOfMutation { get => _propabilityOfMutation; set => _propabilityOfMutation = value; }
        public List<ResultModel> GlobalResultContainerGetSet { get => GlobalResultContainer; set => GlobalResultContainer = value; }
        public int SumOfChromosomeInPopulation { get => sumOfChromosomeInPopulation; set => sumOfChromosomeInPopulation = value; }
        public int TheBestResolve { get => _theBestResolve; set => _theBestResolve = value; }
        public List<RectangleRoom> Rooms { get => _rooms; set => _rooms = value; }
        public List<ControlPointInst> ControlPoints { get => _controlPoints; set => _controlPoints = value; }
        public List<ModelsOfModules> Models { get => _models; set => _models = value; }
        public List<int> ModulesRadius { get => _modulesRadius; set => _modulesRadius = value; }
        public List<double> RatingByOrder { get => _ratingByOrder; set => _ratingByOrder = value; }

        public static SingleSpaceParams getInstance()
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }

        public static void KillSingle()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public static SingleSpaceParams getInstance(int width, int height)
        {
            if (instance == null)
            {
                instance = new SingleSpaceParams(width,height);
            }
            return instance;
        }
    }
}
