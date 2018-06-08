using EvoGen.Helper;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EvoGen.Repository.Collection
{
    public class Molecule : MongoDbBase
    {
        public double Energy { get; private set; }
        public List<Atom> Atoms { get; private set; }
        public List<double> ShapeM { get; private set; }
        public string Nomenclature { get; private set; }
        public bool IsValid { get; set; }

        protected Molecule() { }

        public Molecule(double energy, List<Atom> atoms, List<double> shapeM, bool isValid, long controlId)
        {
            setEnergy(energy);
            setAtoms(atoms);
            setShapeM(shapeM);
            setNomenclature(atoms);
            setIsValid(isValid);
            setGuidString();
            setControlId(controlId);
        }

        public void setEnergy(double energy)
        {
            this.Energy = energy;
        }

        public void setAtoms(List<Atom> atoms)
        {
            Guard.ListNullOrEmpty<Atom>(atoms, "Atoms");
            this.Atoms = atoms;
        }

        public void setShapeM(List<double> shapeM)
        {
            Guard.ListNullOrEmpty<double>(shapeM, "ShapeM");
            this.ShapeM = shapeM;
        }

        private void setGuidString()
        {
            var guid = Guid.NewGuid();
            this.guidString = guid.ToString();
        }

        public void setIsValid(bool isValid)
        {
            this.IsValid = isValid;
        }

        public void setNomenclature(List<Atom> atoms)
        {
            Guard.ListNullOrEmpty(atoms, "Atoms");
            var group = atoms.GroupBy(x => x.ElementSymbol)
                .ToDictionary(x => x.Key, x => x.Count());
            var list = Util.ElementFilterList;
            string nomenclature = "";
            foreach (var symbol in list)
            {
                if (group.ContainsKey(symbol))
                {
                    nomenclature += symbol;
                    if(group[symbol] > 1)
                        nomenclature += group[symbol];
                }
            }
            this.Nomenclature = nomenclature;
        }

        private void setControlId(long controlId)
        {
            Guard.LongLowerThanZero(controlId, "ControlId");
            this.controlId = controlId;
        }
    }
}
