using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoGen.Helper
{
    public class FormulaGenerator
    {
        public static Dictionary<string, int> GenerateFormula()
        {
            CustomRandom customRandom = new CustomRandom();
            var atomsList = Util.OoctetRule.Keys.ToList();
            var molecule = new Dictionary<string, int>();

            var totalAtomsMolecule = customRandom.NextTotalMoleculeAtoms();
            var totalDiferentAtomsMolecule = customRandom.NextDiferentMoleculeAtoms();
            if (totalDiferentAtomsMolecule == 1 && totalAtomsMolecule > 3)
                totalAtomsMolecule = 3;

            var carbonRate = customRandom.NextDouble();
            if (totalDiferentAtomsMolecule > 1 && carbonRate < 0.80)
                molecule.Add("C", 1);

            if (totalDiferentAtomsMolecule > (atomsList.Count / 2))
            {
                foreach (var atom in atomsList)
                {
                    if (!molecule.ContainsKey(atom))
                        molecule.Add(atom, 0);
                }
                while (molecule.Count > totalDiferentAtomsMolecule)
                {
                    var removeAtom = atomsList[customRandom.Next(atomsList.Count)];
                    if (molecule.ContainsKey(removeAtom))
                        molecule.Remove(removeAtom);
                }
            }
            else
            {
                while (molecule.Count < totalDiferentAtomsMolecule)
                {
                    var addAtom = atomsList[customRandom.Next(atomsList.Count)];
                    if (!molecule.ContainsKey(addAtom))
                        molecule.Add(addAtom, 0);
                }
            }
            var moleculeAtoms = molecule.Keys.ToList();
            while (molecule.Sum(x => x.Value) < totalAtomsMolecule)
            {
                var atom = moleculeAtoms[customRandom.Next(moleculeAtoms.Count)];
                molecule[atom] = (molecule[atom] + 1);
            }
            var removeAtoms = molecule.Where(x => x.Value == 0).ToList();
            foreach (var removeAtom in removeAtoms)
            {
                molecule.Remove(removeAtom.Key);
            }
            return molecule;
        }

        public static string GetFormulaFromMolecule(Dictionary<string, int> molecule)
        {
            var atomsList = Util.OoctetRule.Keys.ToList();
            var nomenclature = String.Empty;
            foreach (var atom in atomsList)
            {
                if (molecule.ContainsKey(atom))
                    nomenclature += String.Format("{0}{1}", atom, molecule[atom] > 1 ? molecule[atom].ToString() : "");
            }
            return nomenclature;
        }
    }
}
