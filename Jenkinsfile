pipeline {
  agent any
  stages {
    stage('build') {
      steps {
          script {
			bat '%MSBUILD15% gitrevsubst.sln /p:Configuration=Release'
			
			def slack_template = readFile "${env.WORKSPACE}/changelog/slack_template.mustache"
			def slack_changes = gitChangelog from: [type: 'COMMIT', value: "${GIT_PREVIOUS_COMMIT}"], returnType: 'STRING', template: slack_template, to: [type: 'COMMIT', value: "${GIT_COMMIT}"]
						
			slackSend(teamDomain: 'bhal', token: '4voFmItquQ6QLRJqkhnivRNB', message: "Build Windows Success: (Branch: '${env.GIT_BRANCH}')\n Changes:\n " + slack_changes , baseUrl: 'https://bhal.slack.com/services/hooks/jenkins-ci/', botUser: true, channel: 'jenkins', color: '#00FF00')
			
		  }
      }
    }
  }
}