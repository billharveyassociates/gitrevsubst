pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        bat 'MSBuild gitrevsubst.sln /p:Configuration=Release /p:Platform=\\"Any CPU\\"'
      }
    }
  }
}