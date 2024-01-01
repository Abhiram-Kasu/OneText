

// Get the text content of the page
let text = document.body.textContent;


let regex = /\d+\.\d+/g;

// Create an empty list to store the grades
let grades = [];

// Loop through the matches and push them to the list
let match;
while (match = regex.exec(text)) {
  grades.push(match[0]);
}
grades.pop();
const finalExamNeeded = grades.map((grade) => {

    
    const currentMin = Math.floor(grade / 10) * 10;
    const nextMin = currentMin + 10; 
    
    const staySame = {
      final15Percent: 100 * (currentMin - grade * (1-.15)) / 15,
      final20Percent: 100 * (currentMin - grade * (1-.20)) / 20,
      final25Percent: 100 * (currentMin - grade * (1- .25)) / 25,
    };  
  
    const goUp = {
        final15Percent: 100 * (nextMin - grade * (1-.15)) / 15,
        final20Percent: 100 * (nextMin - grade * (1-.20)) / 20,
        final25Percent: 100 * (nextMin - grade * (1- .25)) / 25,
    };
  
    return {
      originalGrade: grade,
      staySame,
      goUp 
    };
  
  });
  console.log(finalExamNeeded);